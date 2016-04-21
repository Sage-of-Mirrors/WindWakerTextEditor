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

## Textbox Types
The Wind Waker has several types of textboxes. They are described below.
### Dialog
![Normal textbox](http://i.imgur.com/33P79lx.png)

The dialog textbox displays text over a translucent black box. It is used when someone is speaking.
### Wood
![Wood textbox](http://i.imgur.com/y1vocaG.png)

This textbox displays the text over a wooden texture. It is used for text written on wooden signs.
### Stone
![Stone textbox](http://i.imgur.com/qmhCh6q.png)

This textbox displays the text over a stone texture. It is used for text written on stone objects.
### Parchment
(Image pending)

This textbox displays the text over a paper texture. It is used for the letters that can be obtained from the postbox.
### Item Get
![Item Get textbox](http://i.imgur.com/4SUAAlm.png)

This textbox type displays the text over a translucent blue background, with an item image on the left side. It is used to describe an item that the player has just obtained. The item whose image is displayed is determined by the Item ID field in the textbox options.
### Special
![Special textbox](http://i.imgur.com/rSBmE8h.png)

This textbox is similar to the Item Get type above, except for the fact that it doesn't have an item image in it. It is used for narration and status updates - such as when Forest Water expires and returns to being normal water.
### Hint
![Hint textbox](http://i.imgur.com/9zrrUfM.png)

This type is used for hints given to the player by either Tetra or the King of Red Lions through the Pirate's Charm. It appears to be the same as the normal textbox, but it may have certain behaviors that distinguish it.
### Centered Text
To-do.

### Learning a Wind Waker Song
To-do.

### None
![No textbox](http://i.imgur.com/EPLXP2g.png)

This textbox type displays text with no box. Though it used during the prologue, it is glitchy when used in-game.

## Textbox Positions

The textbox options include a field that determines at which predefined position on the screen the textbox draws. These values are described below.

### Top 1
To-do.

### Top 2
To-do.

### Center
To-do.

### Bottom 1
To-do.

### Bottom 2
To-do.

## To-Do
* Dumping of text and message settings to file
* Search and Replace function
* Text color editor
* Highlighting of searched terms when using text search
