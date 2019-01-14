module Main exposing (..)

import Browser
import Html exposing (div, button, text)
import Html.Events exposing (onClick)


main =
    Browser.sandbox { init = 0, view = view, update = update }


view model =
    div []
        [ button [ onClick Increment ] [ text "+" ]
        , div [] [ text (String.fromInt model) ]
        , button [ onClick Decrement ] [ text "-" ]
        ]


type Msg
    = Increment
    | Decrement


update msg model =
    case msg of
        Increment ->
            model + 1

        Decrement ->
            model - 1
