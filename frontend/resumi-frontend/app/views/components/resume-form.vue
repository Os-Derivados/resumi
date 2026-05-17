<template>
	<UForm class="w-full shadow py-8 px-16 my-16" :schema="vm.schema" :state="vm.state" @submit="(e) => e.preventDefault()">
		<UFormField label="Título" name="ownerName" class="w-full">
			<UInput v-model="vm.state.ownerName" :variant="vm.getVariant('ownerName')" @focus="vm.setFocus('ownerName', true)" @blur="vm.setFocus('ownerName', false)" class="w-full" :ui="{ base: 'text-2xl font-bold mb-4' }" />
		</UFormField>

		<ul class="flex gap-8 my-4">
			<li>
				<UFormField label="E-mail" name="email" class="w-full">
					<UInput v-model="vm.state.email" :variant="vm.getVariant('email')" @focus="vm.setFocus('email', true)" @blur="vm.setFocus('email', false)" class="w-full" />
				</UFormField>
			</li>
			<li>
				<UFormField label="Cidade" name="location" class="w-full">
					<UInput v-model="vm.state.location" :variant="vm.getVariant('location')" @focus="vm.setFocus('location', true)" @blur="vm.setFocus('location', false)" class="w-full" />
				</UFormField>
			</li>
			<li>
				<UFormField label="Telefone" name="phoneNumber" class="w-full">
					<UInput v-model="vm.state.phoneNumber" :variant="vm.getVariant('phoneNumber')" @focus="vm.setFocus('phoneNumber', true)" @blur="vm.setFocus('phoneNumber', false)" class="w-full" />
				</UFormField>
			</li>
		</ul>

		<UFormField label="Sobre Mim" name="description" class="w-full mb-4">
			<UInput v-model="vm.state.description" :variant="vm.getVariant('description')" @focus="vm.setFocus('description', true)" @blur="vm.setFocus('description', false)" class="w-full" />
		</UFormField>

		<UFormField label="Palavras-chave" name="keyword" class="w-full">
			<UInput v-model="vm.state.keyword" :variant="vm.getVariant('keyword')" @focus="vm.setFocus('keyword', true)" @blur="vm.setFocus('keyword', false)" class="w-full" />
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
		<h2 class="text-lg font-semibold">Certificados</h2>
		<div class="flex flex-wrap items-center gap-2 mt-2">
			<UInput v-model="certificateFilter" placeholder="Filtrar por tipo, nome ou instituição" class="w-full md:w-96" />
			<UButton label="Novo certificado" icon="i-lucide-plus" color="primary" size="sm" @click="openCreation = !openCreation" />
		</div>

		<div v-if="loadingCertificates" class="text-sm text-slate-500 mt-2">Carregando certificados...</div>
		<ul v-else class="mt-2 space-y-2">
			<li v-if="!filteredCertificates.length" class="text-sm text-slate-500">Nenhum certificado encontrado.</li>
			<li v-for="certificate in filteredCertificates" :key="certificate.id" class="border p-3 rounded-lg">
				<div class="flex justify-between gap-2 items-start">
					<div>
						<p class="font-semibold">{{ certificate.name }}</p>
						<p class="text-xs text-slate-500">{{ certificate.type }} • {{ certificate.institutionName }}</p>
						<p class="text-xs text-slate-500">{{ certificate.startDate }} - {{ certificate.endDate ?? 'Presente' }}</p>
						<p class="text-xs text-slate-500" v-if="certificate.credentialUrl">Link: <a class="text-blue-600" :href="certificate.credentialUrl" target="_blank">Ver credencial</a></p>
					</div>
					<div class="flex gap-2">
						<UButton label="Editar" size="sm" color="secondary" @click="startEdit(certificate)" />
						<UButton label="Excluir" size="sm" color="danger" @click="removeCertificate(certificate.id)" />
					</div>
				</div>
			</li>
		</ul>

		<div v-if="openCreation" class="mt-3 border rounded-lg p-3 bg-slate-50">
			<h3 class="font-semibold mb-2">{{ editingCertificate ? 'Editar certificado' : 'Novo certificado' }}</h3>
			<div class="grid grid-cols-1 md:grid-cols-2 gap-2">
				<UInput v-model="formCertificate.name" placeholder="Nome" />
				<UInput v-model="formCertificate.institutionName" placeholder="Instituição" />
				<UInput v-model="formCertificate.description" placeholder="Descrição" />
				<UInput v-model="formCertificate.location" placeholder="Localização (opcional)" />
				<UInput v-model="formCertificate.startDate" type="date" placeholder="Data início" />
				<UInput v-model="formCertificate.endDate" type="date" placeholder="Data término" />
				<USelect v-model="formCertificate.type" :options="certificateTypeOptions" />
				<UInput v-model="formCertificate.credentialUrl" placeholder="URL do certificado (opcional)" />
			</div>
			<div class="flex gap-2 mt-3">
				<UButton label="Salvar" color="primary" @click="submitCertificate" />
				<UButton label="Cancelar" color="secondary" @click="closeForm" />
			</div>
			<div v-if="formErrors.length" class="mt-2 text-small text-red-600">
				<ul>
					<li v-for="error in formErrors" :key="error">{{ error }}</li>
				</ul>
			</div>
		</div>
	</article>
	</UForm>
</template>

<script setup lang="ts">
import { ref, reactive, computed, onMounted } from 'vue';
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

onMounted(() => {
	loadDegrees();
});
</script>