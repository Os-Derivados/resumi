<template>
	<UForm class="w-full shadow py-8 px-16 my-16" :schema="vm.schema" :state="vm.state"
		@submit="(e) => e.preventDefault()">
		<UFormField label="Título" name="ownerName" class="w-full">
			<UInput v-model="vm.state.ownerName" :variant="vm.getVariant('ownerName')"
				@focus="vm.setFocus('ownerName', true)" @blur="vm.setFocus('ownerName', false)" class="w-full" :ui="{
					base: 'text-2xl font-bold mb-4',
				}" />
		</UFormField>

		<ul class="flex gap-8 my-4">
			<li>
				<UFormField label="E-mail" name="email" class="w-full">
					<UInput v-model="vm.state.email" :variant="vm.getVariant('email')"
						@focus="vm.setFocus('email', true)" @blur="vm.setFocus('email', false)" class="w-full" />
				</UFormField>
			</li>

			<li>
				<UFormField label="Cidade" name="location" class="w-full">
					<UInput v-model="vm.state.location" :variant="vm.getVariant('location')"
						@focus="vm.setFocus('location', true)" @blur="vm.setFocus('location', false)" class="w-full" />
				</UFormField>
			</li>

			<li>
				<UFormField label="Telefone" name="phoneNumber" class="w-full">
					<UInput v-model="vm.state.phoneNumber" :variant="vm.getVariant('phoneNumber')"
						@focus="vm.setFocus('phoneNumber', true)" @blur="vm.setFocus('phoneNumber', false)"
						class="w-full" />
				</UFormField>
			</li>
		</ul>

		<UFormField label="Sobre Mim" name="description" class="w-full mb-4">
			<UInput v-model="vm.state.description" :variant="vm.getVariant('description')"
				@focus="vm.setFocus('description', true)" @blur="vm.setFocus('description', false)" class="w-full" />
		</UFormField>

		<UFormField label="Palavras-chave" name="keyword" class="w-full">
			<UInput v-model="vm.state.keyword" :variant="vm.getVariant('keyword')" @focus="vm.setFocus('keyword', true)"
				@blur="vm.setFocus('keyword', false)" class="w-full" />
		</UFormField>

		<article class="my-8">
			<h2 class="text-lg font-semibold">Experiências</h2>

			<p>Nenhuma experiência adicionada.</p>

			<UButton label="Adicionar experiência" icon="i-lucide-plus" class="mt-4" />
		</article>

		<article class="my-8">
			<h2 class="text-lg font-semibold">Formação Acadêmica</h2>

			<div v-if="!showDegreeForm" class="space-y-4">
				<UCard v-if="degrees.length === 0" class="bg-gray-50">
					<template #header>
						<p class="text-center text-gray-500">Nenhuma formação acadêmica adicionada ainda.</p>
					</template>
					<UButton label="Adicionar formação acadêmica" icon="i-lucide-plus" @click="handleAddDegree" block />
				</UCard>

				<template v-else>
					<DegreeList
						:degrees="degrees"
						:is-loading="isDegreesLoading"
						@add="handleAddDegree"
						@edit="handleEditDegree"
						@delete="handleDeleteDegree" />
				</template>
			</div>

			<div v-else class="bg-gray-50 p-6 rounded-lg border">
				<h3 class="text-md font-semibold mb-4">{{ editingDegree ? 'Editar Formação' : 'Nova Formação Acadêmica' }}</h3>
				<DegreeForm
					:resume-id="resumeId"
					:degree="editingDegree"
					@save="handleSaveDegree"
					@cancel="handleCancelDegreeForm" />
			</div>
		</article>

		<article class="my-8">
			<h2 class="text-lg font-semibold">Atividades Extracurriculares</h2>

			<p>Nenhuma atividade extracurricular adicionada.</p>

			<UButton label="Adicionar atividade extracurricular" icon="i-lucide-plus" class="mt-4" />
		</article>
	</UForm>
</template>

<script setup lang="ts">
import { ResumeFormViewModel } from '../models/resume-form.vm';
import DegreeForm from './degree-form.vue';
import DegreeList from './degree-list.vue';
import type { ReadDegreeModel, CreateDegreeModel, UpdateDegreeModel } from '~/data/api/degree-models';
import { getDegreesAsync, createDegreeAsync, updateDegreeAsync, deleteDegreeAsync } from '~/infra/api/degree-service';

const route = useRoute();
const resumeId = Number(route.query.id);
const vm = new ResumeFormViewModel(resumeId);

// Degree management
const degrees = ref<ReadDegreeModel[]>([]);
const editingDegree = ref<ReadDegreeModel | undefined>(undefined);
const showDegreeForm = ref(false);
const isDegreesLoading = ref(false);

// Load degrees on mount
const loadDegrees = async () => {
	isDegreesLoading.value = true;
	try {
		const result = await getDegreesAsync(resumeId);
		if (result.succeeded && result.data) {
			degrees.value = result.data;
		} else {
			console.error('Erro ao carregar formações:', result.errors);
		}
	} catch (error) {
		console.error('Erro ao carregar formações:', error);
	} finally {
		isDegreesLoading.value = false;
	}
};

// Handlers
const handleAddDegree = () => {
	editingDegree.value = undefined;
	showDegreeForm.value = true;
};

const handleEditDegree = (degree: ReadDegreeModel) => {
	editingDegree.value = degree;
	showDegreeForm.value = true;
};

const handleSaveDegree = async (formData: any) => {
	isDegreesLoading.value = true;
	try {
		if (editingDegree.value) {
			// Update existing degree
			const updateData: UpdateDegreeModel = {
				id: editingDegree.value.id,
				...formData
			};
			delete updateData.resumeId;

			const result = await updateDegreeAsync(resumeId, editingDegree.value.id, updateData);
			if (result.succeeded && result.data) {
				const index = degrees.value.findIndex(d => d.id === editingDegree.value!.id);
				if (index >= 0) {
					degrees.value[index] = result.data;
				}
			}
		} else {
			// Create new degree
			const createData: CreateDegreeModel = formData;
			const result = await createDegreeAsync(resumeId, createData);
			if (result.succeeded && result.data) {
				degrees.value.push(result.data);
			}
		}
		showDegreeForm.value = false;
		editingDegree.value = undefined;
	} catch (error) {
		console.error('Erro ao salvar formação:', error);
	} finally {
		isDegreesLoading.value = false;
	}
};

const handleCancelDegreeForm = () => {
	showDegreeForm.value = false;
	editingDegree.value = undefined;
};

const handleDeleteDegree = async (degreeId: number) => {
	isDegreesLoading.value = true;
	try {
		const result = await deleteDegreeAsync(resumeId, degreeId);
		if (result.succeeded) {
			degrees.value = degrees.value.filter(d => d.id !== degreeId);
		}
	} catch (error) {
		console.error('Erro ao deletar formação:', error);
	} finally {
		isDegreesLoading.value = false;
	}
};

// Load degrees when component mounts
onMounted(() => {
	loadDegrees();
});
</script>