#r @"/Users/gienverschatse/.nuget/packages/fsharp.data/3.3.3/lib/netstandard2.0/FSharp.Data.dll"

open FSharp.Data

type Contacts = CsvProvider<"
    FirstName;MiddleName;LastName;Company;Email;Verified
    Gien;;Verschatse;yes;gien@eightpointsquared.com;yes
    Maria;Francoise;De Rijcke;yes;maria@onesizeforall.com;yes
    Sofie;Claire;Degrootte;no;sofie@outlook.com;no
    Mark;;Vanneste;yes;mark.vanneste@gmail.com;no
    Walter;;Billiet;no;walter.billiet@protonmail.com;yes", ";", HasHeaders=true>

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

let (|HasVerifiedEmail|HasUnverifiedEmail|) contact =
    match contact with
    | Business person
    | Individual person ->
        match person.ContactPreference with
        | Verified _ -> HasVerifiedEmail
        | Unverified _ -> HasUnverifiedEmail

let sendPromotions contacts =
    let verfiedEmailOnly contact =
        match contact with
        | HasVerifiedEmail -> true
        | HasUnverifiedEmail -> false

    contacts
    |> List.filter verfiedEmailOnly
    |> List.map getPromotionTemplate
