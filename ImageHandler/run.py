import imageHandler, os, metric_writer

input_filename = input()
output_filename = input()
def run():
    image_handler = imageHandler.ImageHandler()
    m_writer = metric_writer.MetricWriter()  # Метрика
    if os.path.isfile(input_filename):
        try:
            image_handler.edit_image_file(input_filename, output_filename)
        except Exception as e:
            print(e.args[0])
    else:
        messages = image_handler.edit_images_folder(input_filename, output_filename)
        for message in messages.items():
            print(f'{message[0]}\t{message[1]}')
    m_writer.write_time(image_handler.photos_count)  # Метрика
run()