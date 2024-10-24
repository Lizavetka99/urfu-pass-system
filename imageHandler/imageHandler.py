import numpy as np
import cv2
import os




def check_for_min_quality(image):
    """Возвращает True, если изображение подходит по качеству"""
    height, width = image.shape[:2]
    return height >= 480 and width >= 360  # Данные размеры соответствуют >= 300 dpi для фото 3х4.

def remove_transparency(image, input_path):
    """Возвращает изображение без прозрачности"""
    if image.shape[2] == 4:
        print(f"Изображение {input_path} имеет альфа-канал (прозрачность).")

        # Создаем маску для прозрачных пикселей
        alpha_channel = image[:, :, 3]
        mask = alpha_channel == 0

        # Заменяем прозрачные пиксели на белый цвет
        image[mask] = [255, 255, 255, 255]

        # Удаляем альфа-канал
        image = image[:, :, :3]

    else:
        print(f"Изображение {input_path} не имеет альфа-канала (прозрачности).")

    return image


def identify_face(image):
    """Возвращает True, если одно лицо на изображении"""
    face_count = 0
    prototxt_path = 'weights/deploy.prototxt.txt'
    model_path = 'weights\\res10_300x300_ssd_iter_140000.caffemodel'

    model = cv2.dnn.readNetFromCaffe(prototxt_path, model_path)
    blob = cv2.dnn.blobFromImage(image, 1.0, (300, 300), (104.0, 177.0, 123.0))

    model.setInput(blob)
    output = np.squeeze(model.forward())

    for i in range(0, output.shape[0]):
        confidence = output[i, 2]
        if confidence > 0.8:
            face_count += 1
    return face_count == 1


def turn_face(image):
    """Возвращает изображение лица в нужной ориентации"""
    prototxt_path = 'weights/deploy.prototxt.txt'
    model_path = 'weights\\res10_300x300_ssd_iter_140000.caffemodel'

    model = cv2.dnn.readNetFromCaffe(prototxt_path, model_path)
    h, w = image.shape[:2]
    blob = cv2.dnn.blobFromImage(image, 1.0, (300, 300), (104.0, 177.0, 123.0))

    model.setInput(blob)
    output = np.squeeze(model.forward())

    rotated_image = image
    for i in range(0, output.shape[0]):
        confidence = output[i, 2]
        if confidence > 0.8:
            box = output[i, 3:7] * np.array([w, h, w, h])
            start_x, start_y, end_x, end_y = box.astype(int)
            width = end_x - start_x
            height = end_y - start_y
            print(width, height, width / height)
            if (width / height) >= 1.2:
                if start_x >= width / 3:
                    rotated_image = cv2.rotate(rotated_image,
                                               cv2.ROTATE_90_COUNTERCLOCKWISE)
                else:
                    rotated_image = cv2.rotate(rotated_image,
                                               cv2.ROTATE_90_CLOCKWISE)
        return rotated_image


def check_for_blur(image):
    """Возвращает False, если изображение слишком размыто"""
    minimumQuality = 100

    #  Еще нужно тестить на множестве фотографий, желательно на квадратах лиц.
    #  Минимальное значение "чёткости" пока условно.

    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    return int(cv2.Laplacian(gray, cv2.CV_64F).var()) >= minimumQuality

def get_rectangle(image):
    prototxt_path = 'weights/deploy.prototxt.txt'
    model_path = 'weights\\res10_300x300_ssd_iter_140000.caffemodel'

    model = cv2.dnn.readNetFromCaffe(prototxt_path, model_path)
    h, w = image.shape[:2]
    blob = cv2.dnn.blobFromImage(image, 1.0, (300, 300), (104.0, 177.0, 123.0))

    model.setInput(blob)
    output = np.squeeze(model.forward())
    start_x, start_y, end_x, end_y = 0, 0, 0, 0
    rotated_image = image
    for i in range(0, output.shape[0]):
        confidence = output[i, 2]
        if confidence > 0.8:
            box = output[i, 3:7] * np.array([w, h, w, h])
            start_x, start_y, end_x, end_y = box.astype(int)
    return start_x, start_y, end_x, end_y

def cut_quality(image):
    """Возвращает изображение, урезанное по качеству,
    если изображение слишком большое"""

    x, y, x_b, y_b = get_rectangle(image)


    image_height, image_width = image.shape[:2]

    # Вычисляем ширину и высоту прямоугольника
    w = x_b - x
    h = y_b - y
    print(image_width, image_height)
    # Вычисляем площадь исходного изображения и прямоугольника
    image_area = image_width * image_height
    rect_area = w * h

    # Вычисляем минимальную и максимальную допустимую площадь обрезанного изображения
    min_area = rect_area / 0.8
    max_area = rect_area / 0.6

    # Находим минимальные и максимальные размеры обрезанного изображения
    min_width = int(np.sqrt(min_area * image_height / image_width))
    max_width = int(np.sqrt(max_area * image_height / image_width))
    min_height = int(np.sqrt(min_area * image_width / image_height))
    max_height = int(np.sqrt(max_area * image_width / image_height))



    new_x = x - (w*0.3)
    new_end_x = x_b + (w*0.3)
    new_y = y - (h*0.2)
    new_end_y = y_b + (h*0.2)

    n_w = new_end_x - new_x
    n_h = n_w // 4 * 3
    # Обрезка изображения
    cropped_image = image[ int(new_y) : int(new_end_y), int(new_x):int(new_end_x)]

    return cropped_image


def save_image(image, filename):
    """Сохраняет изображение в папку"""
    output_path = r"C:\Users\shute\Desktop\urfu-pass-system\imageHandler\test_output"
    output_path = os.path.join(output_path, filename)
    cv2.imwrite(output_path, image, [cv2.IMWRITE_JPEG_QUALITY, 100])


input_folder = r"C:\Users\shute\Desktop\urfu-pass-system\imageHandler\Tests"

for filename in os.listdir(input_folder):
    # Полный путь к исходному файлу
    input_path = os.path.join(input_folder, filename)

    # Проверяем, что файл существует и является файлом
    if os.path.isfile(input_path):
        # Читаем изображение с поддержкой альфа-канала
        img = cv2.imread(input_path, cv2.IMREAD_UNCHANGED)
        # Проверяем, что изображение загружено корректно
        if img is not None:
            if not check_for_min_quality(img):
                print("Плохое качество фото", filename)

            img = remove_transparency(img, input_path)
            if not identify_face(img):
                print("Должно быть ровно 1 лицо на фото", filename)
                continue
            img = turn_face(img)
            if not check_for_blur(img):
                print("Изображение слишком размыто", filename)
                continue
            img = cut_quality(img)
            if not check_for_min_quality(img):
                print("Плохое качество фото", filename)

            save_image(img, filename)