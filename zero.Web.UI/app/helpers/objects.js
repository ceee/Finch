
export default {

  setValue(model, props, value)
  {
    const clonedProps = [...props];
    const prop = clonedProps.shift();

    if (!model[prop])
    {
      model[prop] = {};
      //Vue.set(model, prop, {});
    }
    if (!clonedProps.length)
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

    this.setValue(model[prop], clonedProps, value);
  }
};