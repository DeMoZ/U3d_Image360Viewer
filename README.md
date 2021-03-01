# U3d_Image360Viewer
Image and Image360 viewer for Unity3D

**Documentation for Image and 360 Image Viewer asset**

The asset shows and is able to switch between a list of images and 360 images (panorama).

The asset source code is open and can be customized according to your needs.

The asset contains two main objects - one for images and 360 images. Both have some special features.

**Features**

**Images Viewer :**



*   Shows standard photo images. 
*   Zoom in and Zoom out images.
*   Rotates image by 90 degrees to left and right.
*   Move image with swipes according to swipe direction, when image zoomed.
*   Reset button can reset zoom and rotation.
*   Image map shows viewport  position relative to image. That is suitable when the image zoomed and we may need to know what part of the image is seen.

**360 Image Viewer :**



*   Shows full 360 photo images (panorama). 
*   Zoom in and Zoom out 360 images.
*   Move image with swipes according to swipe direction allowing to observe all parts of the 360 image.
*   Reset button can reset zoom and rotation.
*   Image map shows viewport  position relative to image. That is suitable when the 360 image zoomed and/or rotated we may need to know what part of the 360 image is seen.

After importing the asset you will find all the Viewer files at Assets\PhotoViewer folder.

The folder structure:

**The main class **of the asset is PhotoViewer.cs . It receives images as List of ImageData or ImageDate Type. The type contains Sprite and two strings fields - one for image name and other for image date. The example:


<table>
  <tr>
   <td>
    <code>public ImageData Data;</code>
<p>

<code>public List<ImageData> Datas;</code>
<p>
<code>	public PhotoViewer Viewer;</code>
<p>
<code>Viewer.AddImageData(Data);</code>
<p>
<code>     	Viewer.AddImageData(Datas);</code>
<p>
<code>     	Viewer.Show();</code>
   </td>
   <td><code>   [Serializable]</code>
<p>
<code>    public struct ImageData</code>
<p>
<code>    {</code>
<p>
<code>        public Sprite Sprite;</code>
<p>
<code>        public string Name;</code>
<p>
<code>        public string Date;</code>
<p>
<code>    }</code>
   </td>
  </tr>
</table>


ImageViewer contains:

Fields


<table>
  <tr>
   <td>CloseImageViewer
   </td>
   <td>public event perform on viewer closed
   </td>
  </tr>
</table>


Serialised fields


<table>
  <tr>
   <td>PanoramaView _panoramaView
   </td>
   <td>360 Image View - viewer structure object in prefab hierarchy. Shows 360 images
   </td>
  </tr>
  <tr>
   <td>PhotoView _photoView
   </td>
   <td>Image view - viewer structure object in prefab hierarchy. Shows standard images
   </td>
  </tr>
  <tr>
   <td>GameObject _btnRotLeft
   </td>
   <td>The button to Rotate standard image to the left in 90 degrees. Game object in prefab structure.
   </td>
  </tr>
  <tr>
   <td>GameObject _btnRotRight
   </td>
   <td>The button to Rotate standard image to the right in 90 degrees. Game object in prefab structure.
   </td>
  </tr>
  <tr>
   <td>ResetButton _btnReset
   </td>
   <td>The button to Reset rotation/move, zoom, position for current viewer. Game object in prefab structure.
   </td>
  </tr>
  <tr>
   <td>Sprite _imageDefault
   </td>
   <td>Sprite to show when no image is shown in the viewer.
   </td>
  </tr>
  <tr>
   <td>Text _imageName
   </td>
   <td>UI Text object from prefab structure for image name
   </td>
  </tr>
  <tr>
   <td>Text _imageDate
   </td>
   <td>UI Text object from prefab structure for image date
   </td>
  </tr>
  <tr>
   <td>Slider _zoomSlider
   </td>
   <td>UI Slider object from prefab structure for setting zoom on image and 360 image views
   </td>
  </tr>
</table>


Methods


<table>
  <tr>
   <td>AddImageData(ImageData)
   </td>
   <td>Add information for single image
   </td>
  </tr>
  <tr>
   <td>AddImageData(List&lt;ImageData>)
   </td>
   <td>Add list of information for images
   </td>
  </tr>
  <tr>
   <td>CloseViewer
   </td>
   <td>Close image viewer
   </td>
  </tr>
  <tr>
   <td>Clear
   </td>
   <td>Clear image viewer from all added data and reset field
   </td>
  </tr>
  <tr>
   <td>Show
   </td>
   <td>Show first image in the list
   </td>
  </tr>
  <tr>
   <td>NextImage
   </td>
   <td>Show next image from the list
   </td>
  </tr>
  <tr>
   <td>PrevImage
   </td>
   <td>Show previous image from the list
   </td>
  </tr>
  <tr>
   <td>SetZoomSlider
   </td>
   <td>Add delta value to zoom 
   </td>
  </tr>
  <tr>
   <td>SubscribeMeOnNewImage
   </td>
   <td>Subscribe to NewImageShow event
   </td>
  </tr>
  <tr>
   <td>UnSubscribeMeOnNewImage
   </td>
   <td>Unsubscribe from NewImageShow event
   </td>
  </tr>
</table>


For example, reference to TestImageLoader.cs where images are loaded from scriptable object.

The PhotoViewer.cs class has links to two main objects in its prefab structure. Those objects are ImageView.cs for standard images and PanoramaView.cs for 360 images.

When ImageViewer realizes that the next image is a standard image, it disables PanoramaView object, enables ImageView object and initiates with ImageData.

**ImageView.cs **
