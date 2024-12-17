import shutil

input_filename = input()
output_filename = input()
def run():
    shutil.copy(input_filename, output_filename)
run()
