
## Solution (TODO: MOVE THIS TO DIFFERENT FILE)
So w.r.t. the non-digit character replacement, it would be nice to allow the client (the user of the library) to define which character they want and to provide a default should they not care (e.g. if they know they're only passing in strings exclusively made of digits). One problem here is when StringNumber object A's 'replacement character' is different to StringNumber B's and then they're added together. We could throw or just declare that we will default to 'x' or the leftmost operand.

The big question initially I think that needs answering is what is the result of, for example:
        "1a1" + "1a1" and "1a1" * "1a1" 

and the answer I believe is in this specification line:
 - Adding together two StringNumber instances will *add together any characters that are numeric digits that appear in the same position in both strings*. 

So my interpretation of this is:
1. scan both input strings A and B and replace non-digits with the digit 0
1. add the two numbers together numerically 
1. convert to a string (technically a StringBuffer) to get string C
1. scan down both A and B, looking for non-digits
1. for each non-digit found, locate the digit at the same position in C and convert it to the decided-upon character

So, since nouns often translate into classes and actions into methods, what code might we need?
StringNumber class
- should be immutable like a string for consistency (and efficiency (should StringNumber be used at scale) and arguing about correctness)
- methods should allow "fluent syntax" usage e.g. new StringNumber('6') + new StringNumber('2');
- overload the addition operator for natural usage
- overload the multiplication operator for natural usage
- provide constructors that take a string and a long and another StringNumber
- provide a ToString() for general usefulness and debugging
- provide a conversion operator for natural usage with numbers
- arithmetic overflow checking
- declare our contract for handling negative input numbers

I don't see the need for any other classes.