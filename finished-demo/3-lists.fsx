open System

type EmailAddress = EmailAddress of string // single case union

type Email =
    | Verified of EmailAddress
    | Unverified of EmailAddress


type ContactPerson = {
    FirstName : string // AND
    MiddleName : string option // Haskel: Maybe
    LastName : string
    ContactPreference : Email
}

type ContactType =
    | Individual of ContactPerson // OR
    | Business of ContactPerson

let printName person =
    match person.MiddleName with // pattern matching
    | Some middleName ->
        let initial = middleName.Substring(0, 1)
        sprintf "%s %s. %s" person.FirstName initial person.LastName
    | None ->
        sprintf "%s %s" person.FirstName person.LastName

let getPromotionTemplate contact =
    match contact with
    | Individual person ->
        sprintf "Promotion for %s" (printName person) // promoties uitsturen
    | Business person ->
        sprintf "Promotion for your company %s" (printName person)  // bulk promoties uitsturen

// active patterns
// 7
// banana clips
let (|HasVerifiedEmail|HasUnverifiedEmail|) contact =
    match contact with
    | Business person
    | Individual person ->
        match person.ContactPreference with
        | Verified _ -> HasVerifiedEmail
        | Unverified _ -> HasUnverifiedEmail

let contacts =
    [
        { FirstName = "Gien" ; MiddleName = None ; LastName = "Verschatse" ; ContactPreference = EmailAddress("info@eightpointsquared.com")  |> Verified } |> Business
        { FirstName = "Maria" ; MiddleName = None ; LastName = "De rijcke" ; ContactPreference = EmailAddress("maria@onesizeforall.com") |> Verified } |> Business
        { FirstName = "Sofie" ; MiddleName = Some "Claire" ; LastName = "Degrootte" ; ContactPreference = EmailAddress("sofie@outlook.com") |> Verified } |> Individual
        { FirstName = "Walter" ; MiddleName = None ; LastName = "Vanneste" ; ContactPreference = EmailAddress("walter.vanneste@gmail.com") |> Unverified } |> Business
        { FirstName = "Mark" ; MiddleName = None ; LastName = "Billiet" ; ContactPreference = EmailAddress("mark.billiet@gmail.com") |> Unverified } |> Individual
    ]


let sendPromotions contacts =
    let verfiedEmailOnly contact =
        match contact with
        | HasVerifiedEmail -> true
        | HasUnverifiedEmail -> false

    contacts
    |> List.filter verfiedEmailOnly
    |> List.map getPromotionTemplate

sendPromotions contacts;;

// extra
let (|Integer|_|) (str : string) =
    let mutable intvalue = 0
    if Int32.TryParse(str, &intvalue) then Some(intvalue)
    else None

let parseNumeric str =
    match str with
    | Integer i -> printfn "%i : Integer" i
    | _ -> printfn "%s : Not matched." str
