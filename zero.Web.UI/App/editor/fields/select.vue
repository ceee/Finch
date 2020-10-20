 <template>
  <div class="ui-native-select" :disabled="disabled">
    <select :value="value" @input="$emit('input', $event.target.value)" :disabled="disabled">
      <option v-for="item in items" :value="item.key" v-localize="item.value"></option>
    </select>
  </div>
</template>


<script>
  export default {
    props: {
      value: [String, Number, Object],
      items: Array,
      disabled: Boolean
    },
  }
</script>