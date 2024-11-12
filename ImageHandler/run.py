import imageHandler, os

input_filename = input()
output_filename = input()
def run():
    image_handler = imageHandler.ImageHandler()
    if os.path.isfile(input_filename):
        message = image_handler.edit_image_file(input_filename, output_filename)
        print(message)
    else:
        messages = image_handler.edit_images_folder(input_filename, output_filename)
        for message in messages.items():
            print(f'{message[0]}\t{message[1]}')
run()