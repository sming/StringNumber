# String number arithmetic
 - Create a string number library which implements the operations of addition and multiplication on a StringNumber. 
 - A StringNumber instance is created from a string of arbitrary length.
 - Adding together two StringNumber instances will add together any characters that are numeric digits that appear in the same position in both strings. 
 - (Digits are simply [0-9]. The characters [.-] are just regular characters and not digits.) 
 - If either character at that position is not a digit, then the resulting character in that position should be something else. It’s up to you what that is, but please describe your choice.
 - Similar rules apply for multiplication (and this is left intentionally ambiguous).
 - Code must have:
    - unit tests, 
    - good object-oriented design, and have 
    - some amount of “javadoc”. 
- Do not use parseInt or any other large integer library. The goal is to implement the addition and multiplication “algorithm”.

### EXAMPLES
“1” + “2” = “3”

“6” + “7” = “13”

“123” + “9” = “132”

“11111111111111111111” + “22222222222222222222” = “33333333333333333333” 

“a” + “1” = “?” (? is up to you)

“a9” + “13” = “?2” (? is up to you)

“-9” + “13” = “?2” (? is up to you)
