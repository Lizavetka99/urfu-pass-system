import unittest
import face_square_recogniser, image_cutter, quality_checker, transparency_remover, \
    image_rotator, blur_checker
import os
import cv2
import numpy as np


def readImages(directory):
    images = []
    for filename in os.listdir(directory):
        input_path = os.path.join(directory, filename)
        images.append((cv2.imread(input_path, cv2.IMREAD_UNCHANGED), filename))
    return images


class TestCheckForMinQuality(unittest.TestCase):

    def setUp(self):
        self.qChecker = quality_checker.QualityChecker()

    def test_check_for_min_quality_true(self):
        """Проверка, что данные фотографии подходят по размеру."""
        for image, filename in readImages("TestPhotos/TestCheckMinQuality/Valid"):
            self.assertTrue(self.qChecker.check_for_min_quality(image), "Error in " + filename)

    def test_check_for_min_quality_false(self):
        """Проверка, что данные фотографии не подходят по размеру."""
        for image, filename in readImages("TestPhotos/TestCheckMinQuality/InValid"):
            self.assertFalse(self.qChecker.check_for_min_quality(image), "Error in " + filename)


class TestCheckRemoveTransparency(unittest.TestCase):

    def setUp(self):
        self.tRemover = transparency_remover.TransparencyRemover()

    def test_remove_transparency_no_alpha(self):
        for image, filename in readImages("TestPhotos/TestRemoveTransparency/NoAlpha"):
            new_image = self.tRemover.remove_transparancy(image)
            self.assertEqual(new_image.shape[2], 3, "Error in " + filename)

    def test_remove_transparency_with_alpha(self):
        for image, filename in readImages("TestPhotos/TestRemoveTransparency/WithAlpha"):
            new_image = self.tRemover.remove_transparancy(image)
            self.assertEqual(new_image.shape[2], 3, "Error in " + filename)

    def test_remove_transparency_no_channels(self):
        for image, filename in readImages("TestPhotos/TestRemoveTransparency/NoChannels"):
            new_image = self.tRemover.remove_transparancy(image)
            self.assertEqual(new_image.shape[2], 3, "Error in " + filename)


class TestIdentifyFace(unittest.TestCase):

    def setUp(self):
        self.fRecognizer = face_square_recogniser.FaceSquareRecognizer()

    def test_identify_face_no_faces(self):
        for image, filename in readImages(
                "TestPhotos/TestIdentifyFace_noFaces"):
            with self.assertRaises(ValueError) as context:
                self.fRecognizer.get_face_rectangle(image)
            self.assertEqual(str(context.exception), '2', "Error in " + filename)

    def test_identify_face_more_than_one_faces(self):
        for image, filename in readImages(
                "TestPhotos/TestIdentifyFace_More_Than_One_Face"):
            with self.assertRaises(ValueError) as context:
                self.fRecognizer.get_face_rectangle(image)
            self.assertEqual(str(context.exception), '3', "Error in " + filename)

    def test_identify_face_true(self):
        for image, filename in readImages(
                "TestPhotos/TestIdentifyFace_one_face"):
            try:
                self.fRecognizer.get_face_rectangle(image)
            except Exception as e:
                self.fail(f"Method recognize raised exception {e}")

class TestTurnFace(unittest.TestCase):

    def setUp(self):
        self.iRotator = image_rotator.ImageRotator()

    def test_turn_face_should_turn_right(self):
        for image, filename in readImages("TestPhotos/TestImageRotator/RotateRight"):
            new_image = self.iRotator.rotate(image)
            self.assertTrue(np.array_equal(new_image, cv2.rotate(image, cv2.ROTATE_90_CLOCKWISE)), "Error in " + filename)

    def test_turn_face_should_turn_left(self):
        for image, filename in readImages("TestPhotos/TestImageRotator/RotateLeft"):
            new_image = self.iRotator.rotate(image)
            self.assertTrue(np.array_equal(new_image, cv2.rotate(image, cv2.ROTATE_90_COUNTERCLOCKWISE)), "Error in " + filename)

class TestCheckForBlur(unittest.TestCase):

    def setUp(self):
        self.bChecker = blur_checker.BlurChecker()
        self.fsRecogniser = face_square_recogniser.FaceSquareRecognizer()

    def test_check_for_blur_Bad(self):
        for image, filename in readImages("TestPhotos/TestBlurCheck/Bad"):
            face_square = self.fsRecogniser.get_face_rectangle(image)
            # self.assertEqual(self.bChecker.check_for_blur(image, face_square), False, "Error in " + filename)

    def test_check_for_blur_Good(self):
        for image, filename in readImages("TestPhotos/TestBlurCheck/Good"):
            face_square = self.fsRecogniser.get_face_rectangle(image)
            self.assertEqual(self.bChecker.check_for_blur(image, face_square), True, "Error in " + filename)


class TestCutQuality(unittest.TestCase):

    def setUp(self):
        self.iCutter = image_cutter.ImageCutter()

    def test_cut_quality(self):
        for image, filename in readImages("TestPhotos/TestCutQuality"):
            img3x4 = self.iCutter.cut_3x4(image)
            img_cut = self.iCutter.cut_quality(img3x4)
            self.assertEqual(img_cut.shape[:2], (1200, 900), "Error in " + filename)

if __name__ == '__main__':
    unittest.main()