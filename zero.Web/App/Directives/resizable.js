import Vue from 'vue';



/// resize an element
Vue.directive('resizable', {
  bind(el, binding)
  {
    let resizable = new Resizable(el, binding.value);
    resizable.listen();
  },

  update(el, binding)
  {
    
  },

  unbind(el, binding)
  {

  }
});



// object (new) that handles resizing of an element
var Resizable = function (element, params)
{
  const prefix = 'ui.resizable:';
  const cacheKey = prefix + (params.save || 'none');
  const resizingClass = 'ui-resizing';

  let isVertical = ['Y', 'y'].indexOf(params.axis) > -1;

  this.element = element;
  this.params = params;
  this.handle = this.element.querySelector(this.params.handle || '.handle');

  if (!this.handle)
  {
    throw 'Set the [handle] parameter to a valid CSS selector within the attached element';
  }


  // binds events to the handle so the resizing works
  this.listen = () =>
  {
    this.element.addEventListener('mousedown', this.start);
    this.element.addEventListener('dblclick', this.reset);
  };


  // detaches the resizable
  this.detach = () =>
  {
    this.element.removeEventListener('mousedown', this.start);
    this.element.removeEventListener('dblclick', this.reset);
    document.removeEventListener('mousemove', this.resize);
    document.removeEventListener('mouseup', this.stop);
  };


  // start resizing of an element
  this.start = (e) =>
  {
    this.coordinates = {
      X: e.pageX,
      Y: e.pageY,
      offsetX: 0,
      offsetY: 0
    };
    document.body.classList.add(resizingClass);
    document.addEventListener('mousemove', this.resize);
    document.addEventListener('mouseup', this.stop);
  };


  // resize the element
  this.resize = (e) =>
  {
    let deltaX = e.pageX - this.coordinates.X;
    let deltaY = e.pageY - this.coordinates.Y;
    let delta = isVertical ? deltaY : deltaX;
    let newValue = this.value + delta;

    if (params.min && params.min > newValue)
    {
      newValue = params.min;
    }
    else if (params.max && params.max < newValue)
    {
      newValue = params.max;
    }

    this.update(newValue);
  };


  // stop resizing and unbind listeners
  this.stop = (e) =>
  {
    document.body.classList.remove(resizingClass);
    document.removeEventListener('mousemove', this.resize);
    document.removeEventListener('mouseup', this.stop);

    this.value = getCurrentValue();

    localStorage.setItem(cacheKey, this.value);
  };


  // updates dimension of the target element
  this.update = value =>
  {
    if (value > 0)
    {
      this.element.style[isVertical ? 'height' : 'width'] = value + 'px';
    }
  };


  // resets to the original value
  this.reset = () =>
  {
    localStorage.removeItem(cacheKey);
    this.element.style[isVertical ? 'height' : 'width'] = '';
    this.value = getCurrentValue();
  };


  // get current size value from element
  let getCurrentValue = () =>
  {
    return this.element[isVertical ? 'clientHeight' : 'clientWidth'];
  };

  this.value = getCurrentValue();


  // use cached value on load
  if (params.save)
  {
    const newValue = +localStorage.getItem(cacheKey);

    if (newValue > 0)
    {
      this.update(+localStorage.getItem(cacheKey));
      this.value = newValue;
    }
  }
};