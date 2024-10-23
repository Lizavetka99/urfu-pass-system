import numpy as np
import cv2
import os


def check_for_min_quality(image):
    """Возвращает True, если изображение подходит по качеству"""
    pass


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
    pass


def turn_face(image):
    """Возвращает изображение лица в нужной ориентации"""
    pass


def check_for_blur(image):
    """Возвращает False, если изображение слишком размыто"""
    pass


def cut_quality(image, rectangle):
    """Возвращает изображение, урезанное по качеству,
    если изображение слишком большое"""
    x, y, x_b, y_b = rectangle

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

    # Определяем координаты для обрезки
    left = max(0, x - (max_width - w) // 2)
    upper = max(0, y - (max_height - h) // 2)
    right = min(image_width, x_b + (max_width - w) // 2)
    lower = min(image_height, y_b + (max_height - h) // 2)
    print(left, right, upper, lower)
    # Обрезка изображения
    cropped_image = image[upper:lower, left:right]

    return cropped_image


def save_image(image):
    """Сохраняет изображение в папку"""
    output_path = r'E:\Workspace\test_output'
    cv2.imwrite(output_path, image, [cv2.IMWRITE_JPEG_QUALITY, 100])
