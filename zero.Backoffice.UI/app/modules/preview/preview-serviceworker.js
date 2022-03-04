////if ('serviceWorker' in navigator)
////{
////  window.addEventListener('load', () =>
////  {
////    navigator.serviceWorker.register('sw.js').then(
////      registration =>
////      {
////        console.log('Service worker registered with scope: ', registration.scope);
////      },
////      err =>
////      {
////        console.log('ServiceWorker registration failed: ', err);
////      });
////  });
////}

console.info('hi from worker');

self.addEventListener('fetch', event =>
{
  console.info('fetch', event);
  event.request.headers.append("zero-preview", "true");
  event.respondWith(
    fetch(event.request)
    // intercept requests by handling event.request here
  );
});