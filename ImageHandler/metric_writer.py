import time


METRIC_FILE = "metrics.txt"


class MetricWriter:
    def __init__(self):
        self.start_time = time.perf_counter()

    def write_time(self, photo_count):
        final_time = round(time.perf_counter() - self.start_time, 1)
        speed = int(photo_count * 3600 / final_time)
        with open(METRIC_FILE, 'a') as f:
            f.write(f"{photo_count}\t{final_time}\t{speed}\n")
