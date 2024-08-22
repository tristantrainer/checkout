# Checkout System Requirements

## Simple Scanning Total
- [✔] The system should return a total of **zero** when the checkout is empty. 
- [✔] The system should return the **price of a single item** when one item is scanned.
- [✔] The system should return the **total price for all items** when multiple items are scanned.

## Special Price Offers
- [✔] The system should return the **total price for all items** when no special offers are applicable.
- [✔] The system should return the **total price with special offers applied** when eligible items are scanned.
- [✔] The system should correctly calculate and return the **total price with special offers applied** regardless of the order in which items are scanned.

## Price Updates
- [✔] The system should **fix prices at the start of a checkout transaction** rather than using the most recent price updates during the transaction.

## Futher steps not specified in the assignment:
- Implement the price repositories, perhaps poorly named effectively just a list of unit and special prices. The interface defines the contract though so implementation is not restricted i.e. could be a list passed from somewhere else or it could be done based on a transaction approach, fetching prices from a database asof a certain timestamp (when the checkout is created).
