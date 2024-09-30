<template>
    <b-modal class="confirm-delete" id="confirm-modal" v-model="localShow" title="Confirmação" @ok="onConfirm"
        @cancel="onCancel" ok-title="Sim" cancel-title="Cancelar" ok-variant="danger">
        <p>{{ message }}</p>
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
        message: {
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
    },
    methods: {
        onConfirm() {
            this.$emit('confirm');
            this.localShow = false;
        },
        onCancel() {
            this.$emit('cancel');
            this.localShow = false;
        }
    }
};
</script>
<style>
.confirm-delete .modal-title {
    color: #c0392b !important;
    font-size: 1.5rem;
    font-weight: bold;
}
</style>