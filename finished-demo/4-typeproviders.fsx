#r @"/Users/gienverschatse/.nuget/packages/fsharp.data/3.3.3/lib/netstandard2.0/FSharp.Data.dll"

open FSharp.Data

// type providers: hier is een type
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

let getPromotionTemplate contact =
    match contact with
    | Individual person ->
        sprintf "Promotion for %s" person.FirstName // promoties uitsturen
    | Business person ->
        sprintf "Promotion for your company %s" person.FirstName  // bulk promoties uitsturen

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

// sequence: serie van elementen van hetzelfde type
let convert (contacts : Contacts.Row seq) =
    contacts
    |> Seq.map(fun row ->
        let toContactType contact = if row.Company then Business(contact) else Individual(contact)
        let toEmail email = if row.Verified then Verified(email) else Unverified(email)

        let contact = 
            { FirstName = row.FirstName
              MiddleName = if row.MiddleName = "" then None else Some row.MiddleName
              LastName = row.LastName
              ContactPreference = EmailAddress(row.Email) |> toEmail }
        
        contact |> toContactType
    )
    |> Seq.toList

let sendEmails =
    let contacts = Contacts.Load("contacts.csv").Rows
    let convertAndSend = (convert >> sendPromotions)
    contacts |> convertAndSend

sendEmails;;


let seq1 = seq { 1 .. 10 }
printfn "The Sequence: %A" seq1;;

let list1 = [ 1 .. 10]
printfn "The Sequence: %A" list1;;