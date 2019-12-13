===CURTAINS
VAR curtains_count = 0

From a distant viewpoint you see a window.

It is night and their curtains are closed.

You feel full of anticipation. Nervous energy.

*[The curtains open.]

#background:1
The room is well lit and inside you can see somebody moving about.

->MENU

=MENU
{
 -curtains_count < 3:
    ->REAL_MENU
    -else:
    ->FINISH
}
=REAL_MENU

*They move to the left side of the room.
~curtains_count++
    **[They close the door.]
    
    You wonder if there's anyone else in their house? If they are wanting some privacy.

    ->MENU
    
    **[They pick some stuff up from the floor.]
    
    You can't tell what it is. Probably clothes or something like that.
    
    You imagine the room to be quite messy.

    ->MENU
    
    
*They move to the right side of the room.
~curtains_count++
    **[They get into bed.]
    
    They read a book for a while. You can't tell what book it is.
    
    You wonder if they are going to sleep soon.

    ->MENU
    
    **[They open the cupboard.]
    
    They rummage around for something, but you can't see what.

    ->MENU

*[They {stay by|move back to|move back to} the window.]
~curtains_count++
        **[They turn around to face the room.]
        
        You wonder what they are looking at.
        
        ->MENU
        
        **[They stay facing the window and gaze outside.]

        You are worried, for a moment, that they might be able to see you. But you know that they cannot.
        
        ->MENU
        

=FINISH

*They turn off the lights.

{Shutdown()}

->DONE