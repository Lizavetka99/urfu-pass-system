import os
import cv2


class FaceRecognizer:
    def __init__(self):
        self.input_dir = r'Humans_faces_dataset'
        self.output_dir = r'test_output'

    def recognize(self):
        # Загрузка Haar Cascade Classifier для распознавания лиц
        face_cascade = cv2.CascadeClassifier(
            cv2.data.haarcascades + 'haarcascade_frontalface_default.xml')

        # Создание выходной директории, если она не существует
        if not os.path.exists(self.output_dir):
            os.makedirs(self.output_dir)

        # Обработка всех изображений в директории
        for filename in os.listdir(self.input_dir):
            if filename.endswith(('.png', '.jpg', '.jpeg')):
                # Полный путь к изображению
                img_path = os.path.join(self.input_dir, filename)

                # Загрузка изображения
                img = cv2.imread(img_path)

                if img is None:
                    print(f"Error: Unable to load image from {img_path}")
                    continue

                # Преобразование изображения в оттенки серого
                gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

                # Обнаружение лиц
                faces = face_cascade.detectMultiScale(gray, scaleFactor=1.1,
                                                      minNeighbors=5,
                                                      minSize=(30, 30))
                if len(faces) == 0:
                    print(f'Not founded face in {filename}')
                    continue
                if len(faces) > 1:
                    print(f'More than one face in {filename}')
                    continue
                x, y, w, h = faces[0]
                # cv2.rectangle(img, (x, y), (x+w, y+h), (255, 0, 0), 2)
                new_x = x - x * 0.5
                new_y = y - y * 0.8
                new_xw = x + w + (x + w) * 0.1
                new_yh = y + h + (y + h) * 0.3
                if new_xw >= img.shape[1]:
                    new_xw = img.shape[1] - 1
                if new_yh >= img.shape[0]:
                    new_yh = img.shape[0] - 1
                img = img[int(new_y):int(new_yh), int(new_x):int(new_xw)]

                # Сохранение изображения с обнаруженными лицами
                output_path = os.path.join(self.output_dir, filename)
                cv2.imwrite(output_path, img)

                print(f"Processed and saved: {output_path}")

        print("Processing complete.")

