<template>
    <b-modal class="error-modal" id="error-modal" v-model="localShow" title="Erro" hide-footer>
        <p>{{ errorMessage }}</p>
    </b-modal>
</template>

<script>
import { BModal } from 'bootstrap-vue-3';

export default {
    components: { BModal },
    props: {
        show: {
            type: Boolean,
            required: true
        },
        errorMessage: {
            type: String,
            required: true
        }
    },
    data() {
        return {
            localShow: this.show
        };
    },
    watch: {
        show(newVal) {
            this.localShow = newVal;
        },
        localShow(newVal) {
            this.$emit('update:show', newVal);
        }
    }
};
</script>

<style>
.error-modal .modal-title {
    color: red;
    font-size: 1.5rem;
    font-weight: bold;
}
</style>