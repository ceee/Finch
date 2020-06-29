 <template>
  <div class="ui-check-list" :class="{'is-disabled': disabled }">
    <label v-for="item in list" class="ui-native-check ui-check-list-item">
      <input type="checkbox" :checked="isChecked(item)" @input="onChange(item)" />
      <span class="ui-native-check-toggle"></span>
      {{item.name}}
    </label>
  </div>
</template>


<script>
  import { map as _map, filter as _filter } from 'underscore';

  export default {
    name: 'uiCheckList',

    props: {
      value: {
        type: Array,
        default: () => []
      },
      items: {
        type: [Array, Function, Promise],
        required: true
      },
      disabled: {
        type: Boolean,
        default: false
      },
      maxItems: {
        type: Number,
        default: 100
      },
      reverse: {
        type: Boolean,
        default: false
      }
    },


    data: () => ({
      list: []
    }),


    watch: {
      items()
      {
        this.init();
      }
    },


    mounted()
    {
      this.init();
    },


    methods: {

      init()
      {
        if (typeof this.items === 'function')
        {
          this.items().then(res =>
          {
            this.list = res;
          });
        }
        else
        {
          this.list = JSON.parse(JSON.stringify(this.items));
        }
      },

      isChecked(item)
      {
        let index = this.value.indexOf(item.id);
        return (!this.reverse && index > -1) || (this.reverse && index < 0);
      },

      onChange(item)
      {
        let index = this.value.indexOf(item.id);
        let value = JSON.parse(JSON.stringify(this.value));

        if (index < 0)
        {
          value.push(item.id);
        }
        else
        {
          value.splice(index, 1);
        }

        this.$emit('input', value);
      },
    }
  }
</script>

<style lang="scss">
  .ui-check-list
  {
    
  }

  .ui-check-list-item
  {
    display: block;

    & + .ui-check-list-item
    {
      margin-top: 14px;
    }
  }
</style>