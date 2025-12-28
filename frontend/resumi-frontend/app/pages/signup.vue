<template>
	<main class="w-full min-h-screen">
		<header class="text-center w-full h-min">
			<h1 class="text-4xl">Comece Já</h1>

			<p>Registre-se e tenha acesso a melhor plataforma de gestão de currículos.</p>
		</header>

		<div class="flex px-32 py-16 gap-16">
			<UForm :schema="vm.schema" :state="vm.state" class="space-y-4 flex-1 shadow-md w-min">
				<UFormField label="Nome Completo" name="fullName">
					<UInput v-model="vm.state.fullName" />
				</UFormField>

				<UFormField label="Email" name="email">
					<UInput v-model="vm.state.email" />
				</UFormField>

				<UFormField label="Telefone" name="phoneNumber">
					<UInput v-model="vm.state.phoneNumber" />
				</UFormField>

				<UFormField label="Senha" name="password">
					<UInput v-model="vm.state.password" type="password" />
				</UFormField>

				<p>Já possui conta? Acesse nossa
					<ULink to="/login">página de login</ULink>
				</p>

				<UButton type="submit" size="xl">
					Registrar
				</UButton>
			</UForm>

			<aside class="flex-1">
				<header>
					<img src="~/assets/images/signup.svg" alt="">
					<p>Na Resumi, escolhemos o melhor pra você</p>
				</header>

				<p>
					Lorem, ipsum dolor sit amet consectetur adipisicing elit. Iusto reiciendis quod neque, corporis
					autem a voluptates tempora qui esse quos est id beatae! Incidunt vero culpa, voluptates ab natus
					recusandae.
				</p>
			</aside>
		</div>
	</main>
</template>
<script setup lang="ts">
import { SignupViewModel } from '~/views/models/signup.vm';

const vm = new SignupViewModel()
</script>