#pragma once

#include <vector>
#include <algorithm>
#include "Card.h"

class CardDeck {
public:
    std::vector<Card> deck;

    CardDeck();

    std::vector<Card> Shuffle();

    std::vector<Card> Deal(std::vector<Card>& playerHand, int numOfCards);
};
