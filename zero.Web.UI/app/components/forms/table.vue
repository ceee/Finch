 <template>
  <div class="ui-tableeditor" :class="{'is-disabled': disabled }">
    <div class="-table">
      <div v-for="(row, index) in rows" :key="index" class="-row">
        <div v-for="(cell, cellIndex) in row" :key="cellIndex" class="-cell" :class="{'-head': index == 0 }">
          <p class="-content" :data-placeholder="(index == 0 ? 'Title' : 'Content')" contenteditable="true" v-html="cell" @input="onCellChange($event, index, cellIndex)"></p>
        </div>
      </div>
    </div> 
    <div>
      <br><br>
      <input type="number" v-model="idx" placeholder="Index" style="display: inline; width: 80px; margin-right: 10px;" />
      <button type="button" class="ui-button" @click="addRow(idx)">addRow</button>
      <button type="button" class="ui-button" @click="removeRow(idx)">removeRow</button>
      <button type="button" class="ui-button" @click="addColumn(idx)">addColumn</button>
      <button type="button" class="ui-button" @click="removeColumn(idx)">removeColumn</button>
    </div>
  </div>
</template>


<script>
  export default {
    name: 'uiTableEditor',

    props: {
      value: {
        type: Array,
        default: () => []
      },
      disabled: {
        type: Boolean,
        default: false
      }
    },

    data: () => ({
      idx: 0,
      rows: []
    }),

    watch: {
      value(value)
      {
        this.rebuild();
      }
    },

    mounted()
    {
      this.rebuild();
    },

    methods: {

      rebuild()
      {
        this.rows = JSON.parse(JSON.stringify(this.value));
      },

      update()
      {
        this.$emit('input', this.rows);
      },

      onCellChange(ev, rowIndex, cellIndex)
      {
        this.rows[rowIndex][cellIndex] = ev.target.innerText;
        this.update();
      },

      addRow(index)
      {
        this.rows.splice(index, 0, this.rows[0].map(_ => "<br>"));
        this.update();
      },

      removeRow(index)
      {
        this.rows.splice(index, 1);
        this.update();
      },

      addColumn(index)
      {
        this.rows.forEach(row =>
        {
          row.splice(index, 0, "<br>");
        });
        this.update();
      },

      removeColumn(index)
      {
        this.rows.forEach(row =>
        {
          row.splice(index, 1);
        });
        this.update();
      }
    }
  }
</script>

<style lang="scss">
  .ui-tableeditor .-table
  {
    width: 100%;
    border-collapse: collapse;
    break-inside: auto;
    //border: 1px solid var(--color-line-onbg);
    border-radius: var(--radius);
    position: relative;
    display: table;
  }

  .ui-tableeditor .-row
  {
    display: table-row;
  }

  .ui-tableeditor .-cell
  {
    display: table-cell;
    text-align: left;
    width: auto;
    position: relative;
    align-self: start;
    border-top: medium none;
    align-items: start;
    border-right: medium none;
    border-bottom: medium none;
    -moz-box-align: start;
  }

  .ui-tableeditor .-cell.-head .-content
  {
    font-weight: 700;
  }

  .ui-tableeditor .-content
  {
    margin: 0;
    min-height: 1.2em;
    padding: 12px 8px;
  }

  /*.ui-tableeditor .-content[data-placeholder]:before
  {
    content: attr(data-placeholder);
    color: var(--color-text-dim-one);
    position: absolute;
    z-index: 0;
  }*/

  .ui-tableeditor .-content:focus-within
  {
    background: var(--color-bg-dim);
    outline: none;
  }

  .ui-tableeditor .-row
  {
    border-bottom: 1px solid var(--color-line-onbg);
    padding: 10px;
    background: rgb(255, 255, 255) none repeat scroll 0% 0%;
    break-after: auto;
    break-inside: avoid;
  }

  .ui-tableeditor .-row:first-child
  {
    //border-bottom-width: 2px;
  }
</style>