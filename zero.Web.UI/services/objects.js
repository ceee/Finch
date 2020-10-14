
export default {

  setValue(model, props, value)
  {
    const prop = props.shift();

    if (!model[prop])
    {
      model[prop] = {};
      //Vue.set(model, prop, {});
    }
    if (!props.length)
    {
      if (value && typeof value === 'object' && !Array.isArray(value))
      {
        model[prop] = { ...model[prop], ...value };
      } else
      {
        model[prop] = value;
      }
      return;
    }

    this.setValue(model[prop], props, value);
  }
};