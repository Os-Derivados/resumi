<template>
  <div class="degree-list">
    <!-- Lista de Formações Acadêmicas -->
    <div v-if="degrees.length > 0" class="space-y-4">
      <div v-for="degree in degrees" :key="degree.id" class="degree-card border rounded-lg p-4 bg-gray-50 hover:bg-gray-100 transition">
        <div class="flex justify-between items-start mb-2">
          <div>
            <h3 class="text-lg font-semibold">{{ degree.name }}</h3>
            <p class="text-sm text-gray-600">{{ degree.institutionName }}</p>
          </div>
          <div class="flex gap-2">
            <UButton
              size="sm"
              variant="ghost"
              icon="i-lucide-edit"
              @click="emit('edit', degree)"
              :disabled="isLoading">
              Editar
            </UButton>
            <UButton
              size="sm"
              variant="ghost"
              color="red"
              icon="i-lucide-trash"
              @click="handleDelete(degree.id)"
              :disabled="isLoading">
              Deletar
            </UButton>
          </div>
        </div>

        <p class="text-sm text-gray-700 mb-2">{{ degree.description }}</p>

        <div class="grid grid-cols-2 gap-4 text-sm">
          <div>
            <span class="font-semibold">Nível:</span>
            <span class="ml-2">{{ getLevelLabel(degree.level) }}</span>
          </div>
          <div>
            <span class="font-semibold">Local:</span>
            <span class="ml-2">{{ degree.isRemote ? "Remoto" : degree.location || "Presencial" }}</span>
          </div>
          <div>
            <span class="font-semibold">Período:</span>
            <span class="ml-2">{{ formatDate(degree.startDate) }} até {{ degree.stillEngaged ? "Presente" : formatDate(degree.endDate) }}</span>
          </div>
          <div v-if="degree.highlights">
            <span class="font-semibold">Destaques:</span>
            <span class="ml-2 text-gray-600">{{ truncate(degree.highlights, 30) }}</span>
          </div>
        </div>
      </div>
    </div>

    <!-- Mensagem quando vazio -->
    <div v-else class="text-center py-8">
      <p class="text-gray-500 mb-4">Nenhuma formação acadêmica adicionada ainda.</p>
      <UButton
        size="lg"
        icon="i-lucide-plus"
        @click="emit('add')">
        Adicionar Formação Acadêmica
      </UButton>
    </div>

    <!-- Botão para adicionar nova formação (quando já existem) -->
    <div v-if="degrees.length > 0" class="mt-6">
      <UButton
        block
        size="lg"
        variant="soft"
        icon="i-lucide-plus"
        @click="emit('add')"
        :disabled="isLoading">
        Adicionar Outra Formação
      </UButton>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { ReadDegreeModel } from '~/data/api/degree-models';

const degreeLevelMap: Record<string, string> = {
  "HighSchool": "Ensino Médio",
  "Technical": "Técnico",
  "Technologist": "Tecnólogo",
  "Bachelor": "Bacharelado",
  "Master": "Mestrado",
  "Doctorate": "Doutorado"
};

interface Props {
  degrees: ReadDegreeModel[];
  isLoading?: boolean;
}

interface Emits {
  (e: 'add'): void;
  (e: 'edit', degree: ReadDegreeModel): void;
  (e: 'delete', degreeId: number): void;
}

const props = withDefaults(defineProps<Props>(), {
  isLoading: false
});

const emit = defineEmits<Emits>();

const getLevelLabel = (level: string): string => {
  return degreeLevelMap[level] || level;
};

const formatDate = (date?: string): string => {
  if (!date) return "Data indefinida";
  return new Date(date).toLocaleDateString('pt-BR');
};

const truncate = (text: string, length: number): string => {
  return text.length > length ? text.slice(0, length) + "..." : text;
};

const handleDelete = (degreeId: number) => {
  const confirmed = confirm("Tem certeza que deseja deletar esta formação?");
  if (confirmed) {
    emit('delete', degreeId);
  }
};
</script>

<style scoped>
.degree-card {
  animation: fadeIn 0.3s ease-in;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>
