const stripe = require('stripe')('sk_test_51J0pBABEo9A7CLJ3RGTlvYPCSSfzgG2U83linX6Oq2LY8HwO0nFSfuOyt9Phj0fYkbIQVdEzLu0cUzd1BZ1l7QnK00P89ncow5');

const express = require('express');

const app = express();

app.use(express.static('.'));

const YOUR_DOMAIN = 'http://localhost:4242';

app.post('/create-checkout-session', async (req, res) => {

  const session = await stripe.checkout.sessions.create({

    payment_method_types: ['card'],

    line_items: [

      {

        price_data: {

          currency: 'cad',

          product_data: {

            name: 'Stubborn Attachments',

            images: ['https://i.imgur.com/EHyR2nP.png'],

          },

          unit_amount: 2000,

        },

        quantity: 1,

      },

    ],

    mode: 'payment',

    success_url: `${YOUR_DOMAIN}/CheckoutSuccess.html`,

    cancel_url: `${YOUR_DOMAIN}/CheckoutCancel.html`,

  });

  res.json({ id: session.id });

});

app.listen(4242, () => console.log('Running on port 4242'));