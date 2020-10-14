
open System

// variabelen

// inline -> copies the function into the code that uses it and makes it specific

let x = 2.
let multiply x = x * x
multiply 2


// record
// de volgorde is belangrijk!

type BusinessContactPerson = {
    FirstName : string // AND
    MiddleName : string option // Haskel: Maybe
    LastName : string
}

// 1. just all strings
// 2. write printName
// 3. ik wil enkel initial -> error
// 4. option type
// 5. same records

type ContactPerson = {
    FirstName : string // AND
    MiddleName : string option // Haskel: Maybe
    LastName : string
}

let gien : ContactPerson = {
    FirstName = "Gien"
    MiddleName = None
    LastName = "Verschatse"
} 

let maria : BusinessContactPerson = {
    FirstName = "Maria"
    MiddleName = Some "Sofie"
    LastName = "Degrootte"
} 

let printName person =
    match person.MiddleName with // pattern matching
    | Some middleName ->
        let initial = middleName.Substring(0, 1)
        sprintf "%s %s. %s" person.FirstName initial person.LastName
    | None ->
        sprintf "%s %s" person.FirstName person.LastName

printName gien;;
//printName maria;;

// if the compiler is confused, are they really equal?
// let isEqual (p1 : ContactPerson) (c1 : BusinessContactPerson) =
//     if p1 = c1 then
//         printf "Equal"
//     else
//         printf "Not equal"

// isEqual gien maria;;
