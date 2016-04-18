# GC Zelda Text Editor
A text editor for The Legend of Zelda: The Wind Waker, and later, possibly, Twilight Princess.
## Features
### Searching
Messages can be searched by one of the three methods outined below.
#### Text Search
Typing the desired search term into the search box will filter the messages in the message panel so that only those that contain the term are displayed. For example, typing in "Orca" will show only the messages that contain the string "Orca".
#### Message ID Search
The Wind Waker and Twilight Princess assign unique IDs to each message for use in the games' engines. Typing the string "msgid:" followed by the desired message ID will display the message with that ID, if it exists, in the message panel. For example, "msgid:10" will search for a message with the ID 10.
#### Item ID Search
Messages displayed when obtaining an item show the item's image in the textbox. The image to display is determined by the item ID specified in the message's textbox settings. Typing the string "itemid:" followed by the desired item ID will display only the messages that use that ID in the message panel. For example, in The Wind Waker, "itemid:80" will display only the messages with an item ID of 80, which is the Empty Bottle.
## To-Do
* Dumping of text and message settings to file
* Updated UI
* Search and Replace function
* Control code insertion via context menu
* Text color editor
* Highlighting of searched terms when using text search
