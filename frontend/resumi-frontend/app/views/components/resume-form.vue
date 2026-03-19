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
			<p>Nenhuma formação acadêmica adicionada.</p>
			<UButton label="Adicionar formação acadêmica" icon="i-lucide-plus" class="mt-4" />
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
import { getCertificatesAsync, createCertificateAsync, updateCertificateAsync, deleteCertificateAsync } from '~/infra/api/certificate-service';
import type { ReadCertificateModel } from '~/data/api/read-certificate-model';
import type { AddCertificateModel } from '~/data/api/add-certificate-model';
import type { UpdateCertificateModel } from '~/data/api/update-certificate-model';

const route = useRoute();
const resumeId = Number(route.query.id) || 0;
const vm = new ResumeFormViewModel(resumeId);

const certificates = ref<ReadCertificateModel[]>([]);
const loadingCertificates = ref(false);
const openCreation = ref(false);
const editingCertificate = ref<ReadCertificateModel | null>(null);
const certificateFilter = ref('');
const formErrors = ref<string[]>([]);

const certificateTypeOptions = [
	{ label: 'Extracurricular', value: 'Extracurricular' },
	{ label: 'Curso', value: 'Course' },
	{ label: 'Licença', value: 'License' },
	{ label: 'Badge', value: 'Badge' },
	{ label: 'Nomeação', value: 'Nomination' },
];

const formCertificate = reactive<Partial<AddCertificateModel & UpdateCertificateModel>>({
	name: '',
	description: '',
	institutionName: '',
	location: '',
	isRemote: false,
	startDate: new Date().toISOString().slice(0, 10),
	endDate: undefined,
	stillEngaged: false,
	type: 'Extracurricular',
	credentialId: undefined,
	credentialUrl: undefined,
});

const filteredCertificates = computed(() => {
	const filter = certificateFilter.value.trim().toLowerCase();
	if (!filter) return certificates.value;
	return certificates.value.filter(c =>
		c.name.toLowerCase().includes(filter) ||
		c.type.toLowerCase().includes(filter) ||
		c.institutionName.toLowerCase().includes(filter)
	);
});

async function loadCertificates() {
	if (!resumeId) return;
	loadingCertificates.value = true;
	const result = await getCertificatesAsync(resumeId);
	loadingCertificates.value = false;
	if (result.succeeded && result.data) certificates.value = result.data;
}

function validateForm(): boolean {
	const errors: string[] = [];
	if (!formCertificate.name?.trim()) errors.push('Nome obrigatório.');
	if (!formCertificate.description?.trim()) errors.push('Descrição obrigatória.');
	if (!formCertificate.institutionName?.trim()) errors.push('Instituição obrigatória.');
	if (!formCertificate.startDate) errors.push('Data de início obrigatória.');
	if (formCertificate.startDate && formCertificate.endDate && formCertificate.endDate < formCertificate.startDate) errors.push('Data de término não pode ser anterior à data de início.');
	if (!formCertificate.type?.trim()) errors.push('Tipo obrigatório.');
	formErrors.value = errors;
	return !errors.length;
}

function resetForm() {
	formCertificate.name = '';
	formCertificate.description = '';
	formCertificate.institutionName = '';
	formCertificate.location = '';
	formCertificate.startDate = new Date().toISOString().slice(0, 10);
	formCertificate.endDate = undefined;
	formCertificate.stillEngaged = false;
	formCertificate.type = 'Extracurricular';
	formCertificate.credentialId = undefined;
	formCertificate.credentialUrl = undefined;
	editingCertificate.value = null;
	formErrors.value = [];
}

function startEdit(certificate: ReadCertificateModel) {
	editingCertificate.value = certificate;
	openCreation.value = true;
	formCertificate.name = certificate.name;
	formCertificate.description = certificate.description;
	formCertificate.institutionName = certificate.institutionName;
	formCertificate.location = certificate.location ?? '';
	formCertificate.startDate = certificate.startDate;
	formCertificate.endDate = certificate.endDate ?? undefined;
	formCertificate.stillEngaged = certificate.stillEngaged;
	formCertificate.type = certificate.type;
	formCertificate.credentialId = certificate.credentialId;
	formCertificate.credentialUrl = certificate.credentialUrl;
}

async function submitCertificate() {
	if (!validateForm() || !resumeId) return;

	if (editingCertificate.value) {
		const payload: UpdateCertificateModel = {
			id: editingCertificate.value.id,
			name: formCertificate.name,
			description: formCertificate.description,
			institutionName: formCertificate.institutionName,
			location: formCertificate.location,
			isRemote: formCertificate.isRemote,
			startDate: formCertificate.startDate,
			endDate: formCertificate.endDate,
			stillEngaged: formCertificate.stillEngaged,
			type: formCertificate.type,
			credentialId: formCertificate.credentialId,
			credentialUrl: formCertificate.credentialUrl,
		};
		const result = await updateCertificateAsync(resumeId, editingCertificate.value.id, payload);
		if (result.succeeded && result.data) {
			const idx = certificates.value.findIndex(c => c.id === result.data.id);
			if (idx >= 0) certificates.value[idx] = result.data;
			resetForm();
			openCreation.value = false;
		}
		return;
	}

	const payload: AddCertificateModel = {
		resumeId,
		name: formCertificate.name ?? '',
		description: formCertificate.description ?? '',
		institutionName: formCertificate.institutionName ?? '',
		location: formCertificate.location,
		isRemote: formCertificate.isRemote ?? false,
		startDate: formCertificate.startDate ?? new Date().toISOString().slice(0, 10),
		endDate: formCertificate.endDate,
		stillEngaged: formCertificate.stillEngaged ?? false,
		type: formCertificate.type ?? 'Extracurricular',
		credentialId: formCertificate.credentialId,
		credentialUrl: formCertificate.credentialUrl,
	};

	const result = await createCertificateAsync(resumeId, payload);
	if (result.succeeded && result.data) {
		certificates.value.push(result.data);
		resetForm();
		openCreation.value = false;
	}
}

async function removeCertificate(id: number) {
	if (!resumeId) return;
	const result = await deleteCertificateAsync(resumeId, id);
	if (result.succeeded) {
		certificates.value = certificates.value.filter(c => c.id !== id);
	}
}

function closeForm() {
	openCreation.value = false;
	resetForm();
}

onMounted(loadCertificates);
</script>