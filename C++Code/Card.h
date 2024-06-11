#pragma once

#include <iostream>

enum class Rank { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King };
enum class Suit { Clubs, Diamonds, Hearts, Spades };

class Card {
public:
    Rank rank;
    Suit suit;
    bool hold;

    Card(Rank chosenRank, Suit chosenSuit);
};
