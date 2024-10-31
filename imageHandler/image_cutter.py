import cv2
import os


class ImageCutter:
    def __init__(self):
        self.input_folder = r'test_output'
        self.output_folder = r'test_output_cut'

    def cut(self):
        if not os.path.exists(self.output_folder):
            os.makedirs(self.output_folder)

        # Проходим по всем файлам в исходной папке
        for filename in os.listdir(self.input_folder):
            input_path = os.path.join(self.input_folder, filename)

            # Проверяем, что файл существует и является файлом
            if os.path.isfile(input_path):
                # Читаем изображение с поддержкой альфа-канала
                img = cv2.imread(input_path, cv2.IMREAD_UNCHANGED)
                # Проверяем, что изображение загружено корректно
                if img is not None:
                    file_size_bytes = os.path.getsize(input_path)

                    # Изменяем размер изображения
                    height = img.shape[0] // 4
                    width = img.shape[1] // 3
                    part = min(height, width)
                    delta_y = (img.shape[0] - part * 4) // 2
                    delta_x = (img.shape[1] - part * 3) // 2
                    img = img[delta_y : delta_y + part * 4, delta_x : delta_x + part * 3]
                    #img = cv2.resize(img, (part * 3, part * 4))

                    # Полный путь к выходному файлу
                    output_path = os.path.join(self.output_folder, filename)
                    if file_size_bytes > 1000 * 1024:
                    # Сохраняем измененное изображение в формате RGB
                        cv2.imwrite(output_path, img, [cv2.IMWRITE_JPEG_QUALITY, 80])
                    else:
                        cv2.imwrite(output_path, img, [cv2.IMWRITE_JPEG_QUALITY, 100])
                    #print(f"Изображение {input_path} успешно изменено и сохранено.")
                else:
                    print(f"Не удалось загрузить изображение: {input_path}")
            else:
                print(f"Файл не существует или не является файлом: {input_path}")

        print("Все изображения успешно изменены и сохранены.")