**This project is defunct. Please see [Winditor](https://lordned.github.io/Winditor/) for an updated text editor.**

# Wind Waker Text Editor
![Program image](http://i.imgur.com/9kQZtOe.png)

A text editor for The Legend of Zelda: The Wind Waker.
## Features
### Searching
Messages can be searched by one of the three methods outined below.

#### Text Search
Typing the desired search term into the search box will filter the messages in the message panel so that only those that contain the term are displayed. For example, typing in "Orca" will show only the messages that contain the string "Orca".

#### Message ID Search
The Wind Waker assigns unique IDs to each message for use in the games' engines. Typing the string "msgid:" followed by the desired message ID will display the message with that ID, if it exists, in the message panel. For example, "msgid:10" will search for a message with the ID 10.

#### Item ID Search
Messages displayed when obtaining an item show the item's image in the textbox. The image to display is determined by the item ID specified in the message's textbox settings. Typing the string "itemid:" followed by the desired item ID will display only the messages that use that ID in the message panel. For example, in The Wind Waker, "itemid:80" will display only the messages with an item ID of 80, which is the Empty Bottle.

### Adding and Deleting Messages
With this tool, messages may be added to or deleted from the list at will, and each new message will be given a unique message ID for use in-game. This is useful when using custom text with signs or other objects that call text by ID.

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
![Parchment textbox](http://i.imgur.com/QGQsFei.png)

This textbox displays the text over a paper texture. It is used for the text written on paper, such as letters or Sturgeon's tutorial lessons.
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
![Centered textbox](http://i.imgur.com/fjPuibU.png)

This textbox automatically centers the text within it. Besides this, it is the same as the dialog textbox.
### Wind Waker Song
![Wind Waker Song textbox](http://i.imgur.com/8EuZPz5.png)

This box is used for things involving the Wind Waker, such as displaying what song the player just conducted or the pattern to copy when learning a new song.
### None
![No textbox](http://i.imgur.com/EPLXP2g.png)

This textbox type displays text with no box. Though it used during the prologue, it is glitchy when used in-game.

## Textbox Positions

The textbox options include a field that determines at which predefined position on the screen the textbox draws. These values are described below.

### Top 1
![Top 1](http://i.imgur.com/L6UEkir.png)

This places the textbox at the top of the screen. The difference between Top 1 and Top 2 is unknown.
### Top 2
![Top 2](http://i.imgur.com/0vjpjZC.png)

This places the textbox at the top of the screen. The difference between Top 1 and Top 2 is unknown.
### Center
![Center](http://i.imgur.com/0vwBoln.png)

This places the textbox at the center of the screen.
### Bottom 1
![Bottom 1](http://i.imgur.com/zCDS1dK.png)

This places the textbox at the bottom of the screen. The differences between Bottom 1 and Bottom 2 are unknown.
### Bottom 2
![Bottom 2](http://i.imgur.com/XG1IRrt.png)

This places the textbox at the bottom of the screen. The differences between Bottom 1 and Bottom 2 are unknown.

## Control Tags
The games use binary codes throughout the text to modify it. When these codes are rendered in the editor, they are known as Control Tags, and are characterized by text bordered by two chevrons, < and >. Below is a list of these tags and what they do.

### \<color:x\>
\<color:x\> changes the color of the text. X is the index of a color from the color bank to use. By default, in The Wind Waker, the unique colors and their indexes are:

Index | Color
  --- | ---
    0 | White
    1 | Red
    2 | Green
    3 | Blue
    4 | Yellow
    5 | Cyan
    6 | Magenta
    7 | Gray
    8 | Orange
   
### \<icon:x\>
\<icon:x\> displays the image specified by x in the textbox at the position of the Control Tag. These images are mainly restricted to things related to the controller (face buttons, control stick, C-stick, D-pad), but there are other images, including a heart icon and a music note.

### \<wait:x\>
\<wait:x\> causes the text to stop drawing for x amount of time. The units that x represents are unknown.

### \<wait+dismiss:x\>
\<wait+dismiss:x\> is usally used at the end of a textbox, and causes the box to wait for x amount of time before closing itself. The player cannot manually close the textbox before time is up.

### \<wait+dimiss (prompt):x\>
\<wait+dismiss (prompt):x\> is similar to <wait+dismiss:x>. However, here the player is able to manually close the textbox before the specified amount of time has passed.

## To-Do
* Dumping of text and message settings to file
* Search and Replace function
* Text color editor
* Highlighting of searched terms when using text search
