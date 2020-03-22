export default {

  isDebug: process.env.NODE_ENV !== 'production',

  apiBase: '/api',

  countries: [
    { value: 'austria', text: 'Austria' },
    { value: 'germany', text: 'Germany' },
    { value: 'usa', text: 'USA' }
  ],

  categories: [
    { value: 'unknown', text: '' },
    { value: 'socialInsurance', text: 'SVA' },
    { value: 'tax', text: 'Tax' },
    { value: 'payout', text: 'Payout' },
    { value: 'payment', text: 'Payment' }
  ]
};