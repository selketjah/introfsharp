
type ContactPerson = {
    FirstName : string // AND
    MiddleName : string option // Haskel: Maybe
    LastName : string
}

// DU

let gien =
    {
        FirstName = "Gien"
        MiddleName = None
        LastName = "Verschatse"
    }


let printName person =
    match person.MiddleName with // pattern matching
    | Some middleName ->
        let initial = middleName.Substring(0, 1)
        sprintf "%s %s. %s" person.FirstName initial person.LastName
    | None ->
        sprintf "%s %s" person.FirstName person.LastName
