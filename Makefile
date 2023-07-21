IMAGE_NAME = svc-service
IMAGE_TAG = 0.1.2
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



.PHONY: default install restart start stop logs uninstall deploy sh status

default:
	$(DOCKER) build -f $(DOCKERFILE_DIR)/Dockerfile -t $(IMAGE_NAME):$(IMAGE_TAG) $(WORK_DIR)

install:
	$(DOCKER) run $(RUN_OPTIONS) -tid --restart=on-failure --name $(APP_NAME) $(IMAGE_NAME):$(IMAGE_TAG)

restart:
	$(DOCKER) restart $(APP_NAME)

start:
	$(DOCKER) start $(APP_NAME)

stop:
	$(DOCKER) stop $(APP_NAME)

logs:
	$(DOCKER) logs -f $(APP_NAME)

uninstall:
	$(DOCKER) rm -f $(APP_NAME)

tag:
	$(DOCKER) tag $(IMAGE_NAME):$(IMAGE_TAG) $(REPO_NAME)/$(IMAGE_NAME):$(IMAGE_TAG)

publish: tag
	$(DOCKER) push $(REPO_NAME)/$(IMAGE_NAME):$(IMAGE_TAG)

sh:
	$(DOCKER) run $(RUN_OPTIONS) -tid --restart=on-failure --name $(SH_APP_NAME) --entrypoint=/bin/sh $(IMAGE_NAME):$(IMAGE_TAG)

runsh: sh
	$(DOCKER) attach $(SH_APP_NAME)


vglinfo: sh
	$(DOCKER) exec $(SH_APP_NAME) sh /scripts/vglinfo.sh
	$(DOCKER) rm -f $(SH_APP_NAME)

vgltest: sh
	$(DOCKER) exec $(SH_APP_NAME) sh /scripts/vgltest.sh
	$(DOCKER) rm -f $(SH_APP_NAME)

cleansh:
	$(DOCKER) rm -f $(SH_APP_NAME)

status:
	$(DOCKER) ps -a | grep $(IMAGE_NAME)