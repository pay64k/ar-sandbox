import matplotlib.pyplot as plt
import matplotlib.animation as animation
import time
import os
import numpy as np
from scipy import ndimage as ndi
from PIL import ImageOps
from PIL import Image
from scipy.ndimage import gaussian_filter
from skimage.morphology import reconstruction
from skimage.feature import peak_local_max

import atexit
import sys

class ExitHooks(object):
    def __init__(self):
        self.exit_code = None
        self.exception = None

    def hook(self):
        self._orig_exit = sys.exit
        sys.exit = self.exit
        sys.excepthook = self.exc_handler

    def exit(self, code=0):
        self.exit_code = code
        self._orig_exit(code)

    def exc_handler(self, exc_type, exc, *args):
        self.exception = exc

hooks = ExitHooks()
hooks.hook()

def foo():
    if hooks.exit_code is not None:
        print("death by sys.exit(%d)" % hooks.exit_code)
    elif hooks.exception is not None:
        print("death by exception: %s" % hooks.exception)
    else:
        print("natural death")
atexit.register(foo)

fig, axes = plt.subplots(1, 3, figsize=(8, 3), sharex=True, sharey=True,
                         subplot_kw={'adjustable': 'box-forced'})
ax = axes.ravel()
ax[0].axis('off')
ax[0].set_title('Original')

ax[1].axis('off')
ax[1].set_title('Maximum filter')

ax[2].autoscale(False)
ax[2].axis('off')
ax[2].set_title('Peak local max')

fig.tight_layout()



def animate2(i):
    # plt.clf()

    raw_vector = np.loadtxt("map")

    mat = raw_vector.reshape((424, 512))

    rescaled = (255.0 / mat.max() * (mat - mat.min())).astype(np.uint8)

    new_image = Image.fromarray(rescaled)

    inverted_image = ImageOps.invert(new_image)

    image = ndi.gaussian_filter(inverted_image, 1)

    image = gaussian_filter(image, 5)

    seed = np.copy(image)

    seed[1:-1, 1:-1] = image.min()

    mask = image

    dilated = reconstruction(seed, mask, method='dilation')

    peak_image = image - dilated

    image_max = ndi.maximum_filter(peak_image, size=2, mode='constant')

    peak_image = image - dilated
    ax[2].cla()
    coordinates = peak_local_max(peak_image, min_distance=40)

    ax[0].imshow(peak_image, cmap=plt.cm.gray)

    ax[1].imshow(image_max, cmap=plt.cm.gray)

    ax[2].imshow(peak_image, cmap=plt.cm.gray)

    ax[2].plot(coordinates[:, 1], coordinates[:, 0], 'r.')
    print "ok"

# def animate(i):
#     pullData = open("sampleTest.txt","r").read()
#     dataArray = pullData.split('\n')
#     xar = []
#     yar = []
#     for eachLine in dataArray:
#         if len(eachLine)>1:
#             x,y = eachLine.split(',')
#             xar.append(int(x))
#             yar.append(int(y))
#     ax1.clear()
#     ax1.plot(xar,yar)

ani = animation.FuncAnimation(fig, animate2, interval=1000)
plt.show()



