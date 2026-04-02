<template>
  <div class="degree-form space-y-4">
    <UForm :schema="vm.schema" :state="vm.state" @submit.prevent="handleSubmit" class="space-y-4">
      <!-- Nome do Curso -->
      <UFormField label="Nome do Curso" name="name">
        <UInput
          v-model="vm.state.name"
          :variant="vm.getVariant('name')"
          @focus="vm.setFocus('name', true)"
          @blur="vm.setFocus('name', false)"
          placeholder="Ex: Engenharia de Software"
          class="w-full"
          :help="vm.getError('name')" />
      </UFormField>

      <!-- Descrição -->
      <UFormField label="Descrição" name="description">
        <UTextarea
          v-model="vm.state.description"
          :variant="vm.getVariant('description')"
          @focus="vm.setFocus('description', true)"
          @blur="vm.setFocus('description', false)"
          placeholder="Descreva brevemente o curso..."
          class="w-full"
          rows="3"
          :help="vm.getError('description')" />
      </UFormField>

      <!-- Instituição -->
      <UFormField label="Instituição" name="institutionName">
        <UInput
          v-model="vm.state.institutionName"
          :variant="vm.getVariant('institutionName')"
          @focus="vm.setFocus('institutionName', true)"
          @blur="vm.setFocus('institutionName', false)"
          placeholder="Ex: UFRJ"
          class="w-full"
          :help="vm.getError('institutionName')" />
      </UFormField>

      <!-- Local e Remoto -->
      <div class="grid grid-cols-3 gap-4">
        <UFormField label="Localização" name="location" :label-optional="true">
          <UInput
            v-model="vm.state.location"
            :variant="vm.getVariant('location')"
            @focus="vm.setFocus('location', true)"
            @blur="vm.setFocus('location', false)"
            placeholder="Ex: Rio de Janeiro"
            class="w-full"
            :help="vm.getError('location')" />
        </UFormField>

        <div class="flex items-end">
          <UFormField label="Remoto?">
            <UCheckbox v-model="vm.state.isRemote" />
          </UFormField>
        </div>

        <div class="flex items-end">
          <UFormField label="Ainda Cursando?">
            <UCheckbox v-model="vm.state.stillEngaged" />
          </UFormField>
        </div>
      </div>

      <!-- Datas -->
      <div class="grid grid-cols-2 gap-4">
        <UFormField label="Data de Início" name="startDate">
          <UInput
            v-model="vm.state.startDate"
            type="date"
            @focus="vm.setFocus('startDate', true)"
            @blur="vm.setFocus('startDate', false)"
            :variant="vm.getVariant('startDate')"
            class="w-full"
            :help="vm.getError('startDate')" />
        </UFormField>

        <UFormField label="Data de Conclusão" name="endDate" :label-optional="!vm.state.stillEngaged">
          <UInput
            v-model="vm.state.endDate"
            type="date"
            @focus="vm.setFocus('endDate', true)"
            @blur="vm.setFocus('endDate', false)"
            :variant="vm.getVariant('endDate')"
            :disabled="vm.state.stillEngaged"
            class="w-full"
            :help="vm.getError('endDate')" />
        </UFormField>
      </div>

      <!-- Nível de Formação -->
      <UFormField label="Nível de Formação" name="degreeLevel">
        <USelect
          v-model="vm.state.degreeLevel"
          :options="vm.degreeLevelOptions"
          option-attribute="label"
          searchable
          placeholder="Selecione o nível..."
          @focus="vm.setFocus('degreeLevel', true)"
          @blur="vm.setFocus('degreeLevel', false)"
          :help="vm.getError('degreeLevel')" />
      </UFormField>

      <!-- Destaques -->
      <UFormField label="Destaques" name="highlights" :label-optional="true">
        <UTextarea
          v-model="vm.state.highlights"
          :variant="vm.getVariant('highlights')"
          @focus="vm.setFocus('highlights', true)"
          @blur="vm.setFocus('highlights', false)"
          placeholder="Disciplinas importantes, prêmios, etc..."
          class="w-full"
          rows="2"
          :help="vm.getError('highlights')" />
      </UFormField>

      <!-- Botões de Ação -->
      <div class="flex gap-4 justify-end pt-4">
        <UButton
          variant="ghost"
          @click="handleCancel"
          :disabled="vm.isLoading">
          Cancelar
        </UButton>
        <UButton
          type="submit"
          :loading="vm.isLoading">
          {{ vm.isEditMode ? "Atualizar" : "Adicionar" }}
        </UButton>
      </div>
    </UForm>
  </div>
</template>

<script setup lang="ts">
import { DegreeFormViewModel } from '~/views/models/degree-form.vm';
import type { ReadDegreeModel } from '~/data/api/degree-models';

interface Props {
  resumeId: number;
  degree?: ReadDegreeModel;
}

interface Emits {
  (e: 'save', degree: any): void;
  (e: 'cancel'): void;
}

const props = defineProps<Props>();
const emit = defineEmits<Emits>();

const vm = new DegreeFormViewModel(props.degree);

const handleSubmit = async () => {
  if (!vm.validate()) return;

  vm.isLoading = true;

  try {
    const formData = {
      ...vm.getFormData(),
      resumeId: props.resumeId,
      id: props.degree?.id
    };

    emit('save', formData);
  } finally {
    vm.isLoading = false;
  }
};

const handleCancel = () => {
  emit('cancel');
};
</script>

<style scoped>
.degree-form {
  /* Styling here */
}
</style>
