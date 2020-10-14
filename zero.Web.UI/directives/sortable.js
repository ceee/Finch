import Sortable from 'sortablejs';


/// <summary>
/// Enables sorting of a list
/// </summary>
export default {
  name: 'sortable',

  beforeMount(el, binding)
  {
    if (binding.value === binding.oldValue)
    {
      return;
    }

    let sortable = new Sortable(el, binding.value || {});

    //if (this.arg && !this.vm.sortable)
    //{
    //  this.vm.sortable = {};
    //}

    ////  Throw an error if the given ID is not unique
    //if (this.arg && this.vm.sortable[this.arg])
    //{
    //  console.warn('[vue-sortable] cannot set already defined sortable id: \'' + this.arg + '\'')
    //} else if (this.arg)
    //{
    //  this.vm.sortable[this.arg] = sortable;
    //}
  }
};