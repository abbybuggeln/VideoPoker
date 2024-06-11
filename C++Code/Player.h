#pragma once

#include <vector>
#include "Card.h"

class Player {
public:
    std::vector<Card> playerHand;

    void Hold(int cardIndex);

    void ResetHolds();
};
