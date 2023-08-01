IMAGE_NAME = svc-service
IMAGE_TAG = 0.1.7
DOCKERFILE_DIR = .
WORK_DIR = .
REPO_NAME = 192.168.1.104:5000/cloud-collaboration-platform
APP_NAME ?= $(IMAGE_NAME)
SH_APP_NAME = $(APP_NAME)_sh
DOCKER = sudo docker
DEFAULT_RUN_OPTIONS = --cap-add=SYS_PTRACE --gpus all --shm-size=1024m

USE_VGL = true
X = 1

VGL_OPTIONS = --gpus all -v /tmp/.X11-unix/X$(X):/tmp/.X11-unix/X$(X):ro -e VGL_DISPLAY=:$(X)
ifeq ($(USE_VGL),true)
RUN_OPTIONS = $(DEFAULT_RUN_OPTIONS) $(VGL_OPTIONS)
else
RUN_OPTIONS = $(DEFAULT_RUN_OPTIONS)
endif

K8S = kubectl
K8S_REPLICA = 1

.PHONY: default install restart start stop logs uninstall deploy sh status

default:
	$(DOCKER) build -f $(DOCKERFILE_DIR)/Dockerfile -t $(IMAGE_NAME):$(IMAGE_TAG) $(WORK_DIR)

install:
	$(K8S) apply -f deploy.yaml

restart:
	$(K8S) delete pod -f -l 'app=$(APP_NAME)'

start:
	$(K8S) scale deployment -l 'app=$(APP_NAME)' --replicas=$(K8S_REPLICA)

stop:
	$(K8S) scale deployment -l 'app=$(APP_NAME)' --replicas=0

logs:
	$(K8S) logs -f -l 'app=$(APP_NAME)'

uninstall:
	$(K8S) delete -f deploy.yaml

tag:
	$(DOCKER) tag $(IMAGE_NAME):$(IMAGE_TAG) $(REPO_NAME)/$(IMAGE_NAME):$(IMAGE_TAG)

publish: tag
	$(DOCKER) push $(REPO_NAME)/$(IMAGE_NAME):$(IMAGE_TAG)

sh:
	$(DOCKER) run $(RUN_OPTIONS) -tid --restart=on-failure --name $(SH_APP_NAME) --entrypoint=/bin/sh $(IMAGE_NAME):$(IMAGE_TAG)

status:
	$(DOCKER) ps -a | grep $(IMAGE_NAME)