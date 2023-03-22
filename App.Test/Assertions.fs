module Assertions

open DEdge.Diffract
open Xunit
open FsUnit.Xunit
open Swensen.Unquote

type Province =
    | Canton of string
    | State of string

type Address = 
    { Street: string
      HouseNr: string
      PostalCode: string
      Province: Province }
    
type Person =
    { Id: int
      Name: string
      Address: Address
      PreviousAddresses: Address list }

[<Fact>]
let ``Xunit string assertions`` () =
    let expected = "Lorem ipsum dolor sit amet, consectetur adipiscing elit"
    let actual = "Lorem ipsum dolor sxt amet, cnsectetur adipiscing elit"
    
    Assert.Equal(expected, actual)

[<Fact>]
let ``Xunit list assertion`` () =
    let expected = [1; 2; 3; 4; 5]
    let actual = [1; 3; 2; 4; 5]
    
    Assert.True((expected = actual))
    
[<Fact>]
let ``FsUnit sample`` () =
    // https://fsprojects.github.io/FsUnit/
    let expected = [1; 2; 3; 4; 5]
    let actual = [1; 3; 2; 4; 5]
    
    1 |> should not' (equal 2)
    actual |> should equalSeq expected
    
[<Fact>]
let ``FsUnit record sample`` () =
    // https://fsprojects.github.io/FsUnit/
    let expected: Person =
        let previousAddress: Address =
            { Street = "Place st"
              HouseNr = "1a"
              PostalCode = "12345"
              Province = Canton "Zurich" }
        
        let currentAddress =
            { Street = "Main st"
              HouseNr = "25"
              PostalCode = "89762"
              Province = State "Bayern" }
            
        { Id = 1
          Name = "Jaskier"
          Address = currentAddress
          PreviousAddresses = [ previousAddress ] }
        
    let actual: Person =
        let previousAddress: Address =
            { Street = "Place st"
              HouseNr = "1a"
              PostalCode = "12345"
              Province = State "Bayern" }
        
        let currentAddress =
            { Street = "Main st"
              HouseNr = "25"
              PostalCode = "89762"
              Province = Canton "Zurich" }
            
        { Id = 1
          Name = "Jaskier"
          Address = currentAddress
          PreviousAddresses = [ previousAddress ] }
    
    actual |> should equal expected
    
[<Fact>]
let ``Diffract record sample`` () =
    // https://github.com/d-edge/Diffract
    let expected: Person =
        let previousAddress: Address =
            { Street = "Place st"
              HouseNr = "1a"
              PostalCode = "12345"
              Province = Canton "Zurich" }
        
        let currentAddress =
            { Street = "Main st"
              HouseNr = "25"
              PostalCode = "89762"
              Province = State "Bayern" }
            
        { Id = 1
          Name = "Jaskier"
          Address = currentAddress
          PreviousAddresses = [ previousAddress ] }
        
    let actual: Person =
        let previousAddress: Address =
            { Street = "Place st"
              HouseNr = "1a"
              PostalCode = "12345"
              Province = State "Bayern" }
        
        let currentAddress =
            { Street = "Main st"
              HouseNr = "25"
              PostalCode = "89762"
              Province = Canton "Zurich" }
            
        { Id = 1
          Name = "Jaskier"
          Address = currentAddress
          PreviousAddresses = [ previousAddress ] }
    
    
    Differ.Assert(expected, actual)
    
[<Fact>]
let ``Unquote sample`` () =
    let expected = [1; 2; 3; 4; 5]
    let actual = [1; 3; 2; 4; 5]
    
    Assert.Multiple(
        (fun _ -> test <@ 624 / 12 + 184 * 14 = 2 @>),
        (fun _ -> test <@ expected = actual @>)
    )
    
[<Fact>]
let ``Unquote list example`` () =
    let addresses =
        [
            { Street = "Place st"
              HouseNr = "1"
              PostalCode = "2233"
              Province = Canton "Zurich" }
            { Street = "Main st"
              HouseNr = "43b"
              PostalCode = "77845"
              Province = State "Bayern" }
            { Street = "Slow lane"
              HouseNr = "26"
              PostalCode = "3369"
              Province = Canton "Valais" }
            { Street = "Parkway st"
              HouseNr = "0158"
              PostalCode = "12345"
              Province = State "Hamburg" }
            { Street = "Master lane"
              HouseNr = "88"
              PostalCode = "1234"
              Province = Canton "Ticino" }
            { Street = "Central way"
              HouseNr = "43aa"
              PostalCode = "33182"
              Province = State "Hessen" }
            { Street = "Outside st"
              HouseNr = "66"
              PostalCode = "5846"
              Province = Canton "Bern" }
        ]
    
    
    
    test <@
    let stateOnly =
        addresses
        |> List.filter(fun a ->
            match a.Province with
            | State _ -> true
            | Canton _ -> false
            )
    
    let expandSt =
        stateOnly
        |> List.map (fun a ->
            let street = a.Street.Replace(" st", " street")
            { a with Street = street }
        )
        
    let postalCodeLength =
        expandSt
        |> List.forall (fun a -> a.PostalCode.Length = 4)
    postalCodeLength
    @>

[<Fact>]
let ``Unquote record sample`` () =
    let expected: Person =
        let previousAddress: Address =
            { Street = "Place st"
              HouseNr = "1a"
              PostalCode = "12345"
              Province = Canton "Zurich" }
        
        let currentAddress =
            { Street = "Main st"
              HouseNr = "25"
              PostalCode = "89762"
              Province = State "Bayern" }
            
        { Id = 1
          Name = "Jaskier"
          Address = currentAddress
          PreviousAddresses = [ previousAddress ] }
        
    let actual: Person =
        let previousAddress: Address =
            { Street = "Place st"
              HouseNr = "1a"
              PostalCode = "12345"
              Province = State "Bayern" }
        
        let currentAddress =
            { Street = "Main st"
              HouseNr = "25"
              PostalCode = "89762"
              Province = Canton "Zurich" }
            
        { Id = 1
          Name = "Jaskier"
          Address = currentAddress
          PreviousAddresses = [ previousAddress ] }
    
    //App.Test.Check.test <@ expected = actual @>
    test <@ expected = actual @>