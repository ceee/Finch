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
  };


  // detaches the resizable
  this.detach = () =>
  {
    this.element.removeEventListener('mousedown', this.start);
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