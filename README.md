# Demo Task #

In order to better asses your development skills we have prepared requirements for a small demo feature to be implemented.
This demo feature is a simplified and modified real feature of MYBESTBRANDS, so it is related to what we do here.

The purpose of this demo task is to find top and flop products based on traffic data.
We need to track some events on our products and based on this tracking compute some statistics about them. While we also use  tracking solutions from various vendors, this is an internal tracking, where all the parts run our code.

## The requirements:
### Input: 
We need an HTTP endpoint where we record the track events. This endpoint will be called by a client code running in users' browsers. The client code is not in scope of this project, but the server side HTTP Endpoint is. The calls to the HTTP Endpoint have the folowing format:

http://HOST_AND_PORT/track?eventtype=EVENT_TYPE_VALUE&ids=COMMA_SEPARATED_LIST_OF_IDS

...where
 * HOST_AND_PORT are free to choose for the purpose of this demo. Localhost on port 80 is good enough.
 * COMMA_SEPARATED_LIST_OF_IDS is as the name suggests, a comma separated list of positive integer ids. There must be at least one id there. If there are many ids (up to 40 for now), the list is short enough to fit in browser supported url lengths. The last ID is not followed by comma.
 * EVENT_TYPE_VALUE is the kind of event we are tracking. This can have one of the following values (without the quotes):
   - "list": tracks a page impression of multiple products on a list list page. The products are specified by ids in the COMMA_SEPARATED_LIST_OF_IDS value of ids parameter. Example: http://127.0.0.1/track?eventtype=list&ids=123456,1324,586632514658,45650,547898,87954,987987
   - "details": tracks a view of one product's details page. In this case the "ids" parameter has a single id value. Example: http://127.0.0.1/track?eventtype=details&ids=123456
   - "conversion": tracks a conversion of one product. In this case the "ids" parameter has a single id value. Example: http://127.0.0.1/track?eventtype=conversion&ids=123456

Other values are not supported. Invalid combinations are not supported. An unsupported input must lead to a 400 response status.
Valid requests must reponse with a 200 OK status code and json payload containing the number of tracked products. Example response payload: {"OK": 25}

### Output:
We need a daily export with some statistics for the products. This daily export must be performed by a command line tool, which takes a date in format "YYYY-MM-DD" like (2017-03-15 for March 15th this year) as an optional parameter. If missing, today must be assumed. The data parameter represents the exclusive upper bound of the statistics. 
That is, no data of the specified date is included in the results, but only data prior to this date. See section column descriptions below for which intervals are relevant.
The name of the exported file must be specified by parameter and it must be in CSV format with the following headers:

"ID","#List Impressions","#Details Views","#Conversions","Click Rate 7 Days","Conversion Rate 7 Days","Conversion Rate 14 Days"

The values of the columns are as following:
 * "ID": this is the product id
 * "#List Impressions": number of list impressions of the corresponding product for the complete day previous to the given parameter or yesterday if no parameter given.
 * "#Details Views": number of details views, by the same rules as "#List Impressions".
 * "#conversions": number of conversions, by the same rules as "#List Impressions".
 * "Click Rate 7 Days": is the proportion of details views over list views over the previous 7 days. Upper bound is the given date, but excluding data of the given date. This is a percentage value with maximum 2 digits after comma. Cap this value to 100%.
 * "Conversion Rate 7 Days": is the proportion of conversions over the sum of details views and list impressions of the corresponding product, over the previous 7 days. Interval, number format and uppoer bound by the same rules as "Click Rate 7 Days".
 * "Conversion Rate 14 Days": just like the "Conversion Rate 7 Days" but computed over a 14 days interval.
 
Each line in the file refers to one product and there is at most one line per product in the exported file. Products without event in the relevant intervals are not present in the exported file. 
 
### Constraints:
 * It must be possible to deploy the HTTP Endpoint service on multiple machines.
 * The HTTP Endpoint service must be robust against wrong input and tolerant to transient I/O problems.
 * It is expected to have up to 200000 HTTP requests per day on the tracking endpoint. These requests are unevenly distributed over the day. List tracking requests have an expected average of 10 product ids.
 * It is expected to run the CSV export multiple times for the same input. Use case: answer this question "What were the numbers last Sunday?" These runs with the same input should produce the same results.
 * You may freely choose the technology for both the HTTP Endpoint and the command line tool for exports. Any dependencies to install must be provided or specified.
 * You may freely choose any persistence technology (files, RDBMS, NoSql etc.), as long as it satisfies the other requirements. If a database is setup, db scripts should be provided.
 * You should create the solution by yourself, and we shall discuss it together after reviewing it.
 * You are free to choose the testing strategy and the tools. There is no requirement for 100% test coverage.
 * You may generate data as you need for checking your solution.  
 * You need to provide the source code and any additional info in order for us to build and run the solution.
 * You should send the solution until Wednesday, 21st of June, by the end of the day. You may send it anytime earlier, though. The sooner the better.

 ## Solution
 The events are logged by a ASP.NET Web API 2 web application to a SQL Server database. The exporting is done by a simple .NET command line tool. 

 ### Development Tools
 * Visual Studio 2017 Community
 * .NET Framework 4.7
 * ASP.NET Web API 2
 * MS SQL Server 2016
 * NuGet Package Manager
 * locust.io for load testing
 * Azure App Service and SQL Database
 * Libraries: Entity Framework, Unity (DI)
 * Test libraries: MSTest, NSubstitute, Effort

 ### Structure
 Solution file: `src/Zeus.sln`.

 * `Zeus.Common`: shared classes
 * `Zeus.Exporter`: command line tool for exporting the data into csv format
 * `Zeus.SqlTracker`: tracks events into a SQL database
 * `Zeus.Web`: HTTP endpoint
 
 Unit Tests projects:
 * `Zeus.Exporter.Tests`
 * `Zeus.SqlTracker.Tests`
 * `Zeus.Web.Tests`

 #### Web
 Contains the HTTP endpoint. It's using the `ModelBinders` to convert the query parameters. Has a simple endpoint in the `TrackController`. It's using an `ITracker` to track the events.
 Dependency injection is done using the Unity framework. 

 #### SqlTracker
 Tracks events into a MS SQL database. It's using Entity Framework 6.1.3 for ORM mapping. The `create.sql` script can be used to create the database tables. The summarization is done in the SQL server, the query is created using Linq. 

 #### Exporter: 
 Usage: `.\exporter.exe` or `.\exporter.exe 2017-06-19`

 Creates the file in the current directory. The name of the file is `events_{date}.csv`.
 If the file already exists, it's overwritten without notice.
 
 ### Load Testing in Azure
 The solution was deployed to Azure, using App Services and SQL databases.

 Configuration:
 * Web Server: 4 Core, 7 GB RAM
 * SQL Server: 100 DTU/s

 Load Testing with 2 computers:
 * 250 requests / sec
 * locustio configuration in the file `locustfile.py`
 * 50,000 different product ids
 * 10 products / requets
 * test running for about an hour
 
 Results:
 * over 10,000,000 rows in the database
 * Web server utilization: 25% memory, 50% CPU
 * Average response time: 200-400ms
 * 15,000 requests / min
 * 1,000,000 requests total
 * exporting [1.5MB file](https://github.com/mdavid626/zeus/blob/master/doc/events_2017-06-19.csv?raw=true) took 50s (100 DTU/s) and 13s (1000 DTU/s)
 * final DB size: 400 MB
 
 #### CPU and Memory utilization
  ![CPU/Memory](https://github.com/mdavid626/zeus/blob/master/doc/cpu.jpg?raw=true)

 #### Final database size
 ![DB Size](https://github.com/mdavid626/zeus/blob/master/doc/dbsize.jpg?raw=true)

 #### DTU percentage
 ![DTU percentage](https://github.com/mdavid626/zeus/blob/master/doc/dtu.jpg?raw=true)

 #### Count of requests 
 ![Requests count](https://github.com/mdavid626/zeus/blob/master/doc/requestscount.jpg?raw=true)

 #### Average response time
 ![Response time](https://github.com/mdavid626/zeus/blob/master/doc/responsetime.jpg?raw=true)