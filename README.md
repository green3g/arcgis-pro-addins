Pro Addins
==========

A collection of ArcGIS Pro buttons bundled into an addin.

## Streetview

Open Google streetview on a map click point

![Google Streetview](./images/streetview.gif)

## Definition Query Buttons

Buttons to set a definition expression on a layer to the selected features. Similar to Create layer from selection. The only difference is the definition query is placed on the selected layer, not on a newly created layer.

![Definition Expression Button](./images/select.gif)

## External Viewer Button

A configureable button that allows a user to open an external application using an ID property and a url.

This button must be enabled first using the Pro Options dialog and has the following configurable settings:

 - Application URL: The url to the application. Be sure to include the text {0} which will be replaced with the ID property of the selected feature
 - Layer ID Field: The field name to query in the selected feature
 - Layer Name: The name of the layer to query in the map
