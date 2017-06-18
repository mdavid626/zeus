from locust import HttpLocust, TaskSet, task
import random

class UserBehavior(TaskSet):
    @task(1)
    def index(self):
        type = random.sample(["list", "details", "conversion"], 1)[0];
        ids = ",".join([str(i) for i in random.sample(range(1,50000), 10)])
        self.client.get("/track?ids=" + ids + "&eventType=" + type)

class WebsiteUser(HttpLocust):
    task_set = UserBehavior
    min_wait = 5000
    max_wait = 9000