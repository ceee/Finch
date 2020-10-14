 <template>
  <div class="ui-alias" :class="{'is-disabled': disabled, 'is-locked': locked }">
    <button type="button" class="ui-alias-lock" @click="toggleLock">
      <i :class="locked ? 'fth-lock' : 'fth-unlock'"></i>
    </button>
    <input ref="input" :value="value" @input="onChange" type="text" class="ui-input" :maxlength="maxLength" :readonly="locked" :disabled="disabled" @blur="locked=true" />
  </div>
</template>


<script>
  import { map as _map } from 'underscore';
  import UtilsApi from '@zero/resources/utils.js';

  export default {
    name: 'uiAlias',

    emits: ['input'],

    props: {
      value: {
        type: String,
        default: null
      },
      name: {
        type: String,
        required: true,
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      },
      maxLength: {
        type: Number,
        default: 200
      }
    },


    data: () => ({
      custom: false,
      locked: true
    }),


    watch: {
      name: function(val)
      {
        this.updateAlias(val);
      }
    },


    mounted()
    {
      this.updateAlias(this.name);
    },


    methods: {

      onChange(ev)
      {
        let alias = ev.target.value;

        this.custom = alias !== this.value;
        this.updateAlias(alias);

        this.$emit('input', alias);
      },


      toggleLock()
      {
        this.locked = !this.locked;

        if (!this.locked)
        {
          this.$nextTick(() =>
          {
            this.$refs.input.focus();
            this.$refs.input.select();
          });
        }
      },


      updateAlias(value)
      {
        UtilsApi.generateAlias(value).then(alias =>
        {
          this.custom = alias !== this.value;
          this.$emit('input', alias);
        });
      }
    }
  }
</script>

<style lang="scss">
  .ui-alias
  {
    display: flex;
    align-items: center;

    button
    {
      width: 24px;
      height: 24px;
      border-radius: var(--radius);
      background: var(--color-box-nested);
      display: inline-flex;
      justify-content: center;
      align-items: center;
      font-size: 13px;
      margin-right: 10px;
    }

    input
    {
      background: transparent !important;
      border: none !important;
      box-shadow: none !important;
      height: 24px !important;
      padding: 0 !important;
      outline: none !important;
      min-width: 10px !important;
      width: auto !important;
    }

    &:not(.is-locked) input
    {
      font-weight: bold;
    }
  }
</style>