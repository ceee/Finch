
import Vue from 'vue';
import VueRouter from 'vue-router';


export default class ZeroBootstrapper
{
  _vue : Vue;
  _router : VueRouter;


  constructor(vue: Vue, router: VueRouter)
  {
    this._vue = vue;
    this._router = router;
  }


  run()
  {
    
  }
}